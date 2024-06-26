using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Splatterbot.Functions;
using Splatterbot.Commands;
using Splatterbot.LargeArrayData;
using Discord.Interactions.Builders;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.ComponentModel.DataAnnotations;
using Discord.Net;
using Discord.Interactions;

namespace Splatterbot
{

    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        //~~~~~~~~~~~~~~~~~~DECLARATION~~~~~~~~~~~~~~~~~~~~~~~~~//
        public static Random rnd = new Random(); //Initialization of random object used to generate random numbers
        public static int randomNum;
        //static private readonly IConfiguration _config;
        static dynamic token;// = JsonConvert.DeserializeObject<dynamic>("config.json");


        private DiscordSocketClient _client;
        private CommandService _commands;
        private InteractionService _interaction;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {

            //~~~~~~~~~~~~~~~~~~INITIALIZATION~~~~~~~~~~~~~~~~~~~~~~~~~~//
            //Directory.SetCurrentDirectory(dir);
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _interaction = new InteractionService(_client);

            //The singletons in the services variable ensure that only one instance of the _client and _commands are created
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<InteractionService>(p => new InteractionService(p.GetRequiredService<DiscordShardedClient>()))
                .BuildServiceProvider();

            string dir = Environment.CurrentDirectory; //Gets current directory of project folder
            StreamReader r = new StreamReader(dir + "//config.json"); 
            string jsonString = r.ReadToEnd();

            token = JsonConvert.DeserializeObject<dynamic>(jsonString);

            _client.Log += _client_Log;
            _client.Ready += _client_Ready;
            _client.InteractionCreated += InteractionHandler;
            _interaction.SlashCommandExecuted += SlashCommandExecuted;

            await _interaction.AddModulesAsync(Assembly.GetEntryAssembly(), _services); //adds commands to the service module
            //await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token["key"].ToString());
            await _client.StartAsync();
            await Task.Delay(-1); //Prevents bot from closing immediately

        }

        //Registers the commands created eitehr globally or server specific
        public async Task _client_Ready() {

            //await _client.Rest.DeleteAllGlobalCommandsAsync(); // Clear global command chache

            await _interaction.RegisterCommandsGloballyAsync(); //register commands globally
            //await _interaction.RegisterCommandsToGuildAsync(459193626736721921); //testing slash commands

        }

        //Error catching that occurs whenever a command is executed
        private async Task SlashCommandExecuted(SlashCommandInfo scInfo, IInteractionContext iContext, Discord.Interactions.IResult iResult)
        {
            if (!iResult.IsSuccess)
            {
                switch (iResult.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await iContext.Interaction.RespondAsync($"Unmet Precondition: {iResult.ErrorReason}");
                        break;
                    case InteractionCommandError.UnknownCommand:
                        await iContext.Interaction.RespondAsync("Unknown command");
                        break;
                    case InteractionCommandError.BadArgs:
                        await iContext.Interaction.RespondAsync("Invalid number or arguments");
                        break;
                    case InteractionCommandError.Exception:
                        await iContext.Interaction.RespondAsync($"Command exception: {iResult.ErrorReason}");
                        break;
                    case InteractionCommandError.Unsuccessful:
                        await iContext.Interaction.RespondAsync("Command could not be executed");
                        break;
                    default:
                        break;
                }
            }
        }

        //Handler for what to do when a command is sent to the bot
        private async Task InteractionHandler(SocketInteraction si) {
            
            //Run slash command 
            try
            {
                var ctx = new SocketInteractionContext(_client, si);
                await _interaction.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //If slash command fails, the default response we set up at the beginning of the command may still exist, so delete the response
                var ctx = new SocketInteractionContext(_client, si);
                if (si.Type == InteractionType.ApplicationCommand)
                    await si.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }

            _client.ButtonExecuted += menuButtonHandler; //When buttons are clicked, refer to the menuButton Handler to see what to do
            _client.SelectMenuExecuted += myMenuHandler; //When Dropdown Menus are clicked, refer to the myMenuHandler Handler to see what to do
            _client.UserJoined += userJoined; //When bot joins server, refer userJoined to see printed out text
        }




        /*public async Task RegisterCommandsAsync()
        {

            _client.MessageReceived += HandleCommandAsync; //Handle for when the bot receives a message
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        }*/

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task MessageReceivedAsync(SocketMessage arg)
        {
            var command = arg as SocketUserMessage;
            var context = new CommandContext(_client, command);

            if(command.Content.Equals("dye"))
            {
                await context.Channel.SendMessageAsync("no u");
            }
            //command implementation here
        }

        //FUNCTION: HandleCommandAsync
        //DESCRIPTION: Determines what actions the bot will take when it receives a message depending on the messages contents
        /*private async Task HandleCommandAsync(SocketMessage arg)
        {
            var command = arg as SocketUserMessage;
            var context = new CommandContext(_client, command);

            if (command.Author.IsBot) return; //If the command being sent to the bot is another bot, ignore the command

            int argPos = 0;


            if (command.HasStringPrefix("$$", ref argPos))
            { //To issue a command to the bot, "$$" is the prefix you will place before you try calling a command
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason); //Debug Line for any errors

            }
            //if a real command isn't sent, check for jokes
            else {
                //Handler for whenever someone sends a message with a word ending ending in -er or -or
                string[] words = command.Content.Split(new char[] {' '}); //contains the user turned into an array divided by spaces
                
                //loops through each word in the sentence until it finds the first word that ends in -er or -or
                for(int i = 0; i < words.Length; i++){
                    //await context.Channel.SendMessageAsync(words[i]);
                    
                    //first checks word length to make sure its not one char
                    if(words[i].Length > 1){
                        //await context.Channel.SendMessageAsync(words[i][0].ToString());
                        //if the length is greater than one, check the last two characters to see if it ends in er or or
                        if(
                            (words[i][words[i].Length-2].ToString().Equals("e") || (words[i][words[i].Length-2].ToString().Equals("o")))
                            &&
                            (words[i][words[i].Length-1].ToString().Equals("r"))
                        ){
                            await context.Channel.SendMessageAsync(words[i] + "? I barely know her!");
                            break;
                        }
                    }
                }
            }

            _client.ButtonExecuted += menuButtonHandler; //When buttons are clicked, refer to the menuButton Handler to see what to do
            _client.SelectMenuExecuted += myMenuHandler; //When Dropdown Menus are clicked, refer to the myMenuHandler Handler to see what to do
            _client.UserJoined += userJoined; //When bot joins server, refer userJoined to see printed out text
        }*/

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EVENT HANDLER FUNCTIONS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //Event Handler for what happens when the buttons for the splatnet menu are selected
        public async Task menuButtonHandler(SocketMessageComponent component)
        {

            //Sets up variables that will be use to control aspects of the menu
            Discord.EmbedBuilder embed = new Discord.EmbedBuilder { }; //Menu display
            dynamic builder = new ComponentBuilder(); //Button Display

            int i; //variable used to determine which item is currently being shown

            // Change control variable based on which custom id triggered the event
            switch (component.Data.CustomId)
            {
                //Change :
                case "splatnet0":
                    i = 0;
                    break;
                case "splatnet1":
                    i = 1;
                    break;
                case "splatnet2":
                    i = 2;
                    break;
                case "splatnet3":
                    i = 3;
                    break;
                case "splatnet4":
                    i = 4;
                    break;
                case "reroll":
                    i = 5;
                    break;
                default:
                    i = -1;
                    break;
            }

            //Choose which embed and button to build based on i value
            if (i >= 0 && i <= 4)
            { //This option is for writing the menu based on splatnet store display
                embed = updateSplatnetEmbed(i);
                builder = updateSplatnetButtons(i);
            }
            else if (i == 5)
            { //This option is for writing the menu based on the random weapon generation
                embed = UpdateRandomEmbed();
            }

            await component.UpdateAsync(
                msg => {
                    msg.Embed = embed.Build();
                    if (i != 5)
                        msg.Components = builder.Build();
                });
        }

        //Event Handler for the help function menu
        public async Task myMenuHandler(SocketMessageComponent component)
        {
            string text = string.Join(", ", component.Data.Values); //reads which input the user wishes to learn about
            int i; //Control variable used to determine which data in the text array will be printed out onto the screen
                   //await arg.RespondAsync($"You have selected {text}");

            Discord.EmbedBuilder embed = new EmbedBuilder { }; //Text display
            Discord.ComponentBuilder menuBuilder = new ComponentBuilder(); //Menu Display

            switch (text)
            {
                //value :
                case "map":
                    i = 1;
                    break;
                case "salmon":
                    i = 2;
                    break;
                case "splatnet":
                    i = 3;
                    break;
                case "misc":
                    i = 4;
                    break;
                default:
                    i = -1;
                    break;
            }

            embed = updateHelpEmbed(i);
            menuBuilder = updateHelpMenu(i);

            await component.UpdateAsync(
                    msg => { msg.Embed = embed.Build(); msg.Components = menuBuilder.Build(); });
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~HELPER FUNCTIONS FOR SPLATNET~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //Helper function used to update the item display when the splatnet function is called
        public Discord.EmbedBuilder updateSplatnetEmbed(int i)
        {
            dynamic net = JsonGetters.getNet();
            //Implements a feature where text will change depending on a given emote allowing the bot to essentially create a menu

            double timeLeft = (double)net["merchandises"][i]["end_time"] - (double)DateTimeOffset.Now.ToUnixTimeSeconds();

            double hours = Math.Floor(timeLeft / 3600);
            double mins = Math.Floor((timeLeft / 60)) - (60 * hours);
            double seconds = (timeLeft % 60);

            string imageURL = "https://splatoon2.ink/assets/splatnet" + net["merchandises"][i]["gear"]["image"];
            string cost = net["merchandises"][i]["price"] + "\t\t";
            string main = net["merchandises"][i]["skill"]["name"] + "\t\t";
            string sub = net["merchandises"][i]["gear"]["brand"]["frequent_skill"]["name"] + "\t\t";

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

            var embed = new Discord.EmbedBuilder { };
            embed
                 .WithTitle("**__" + net["merchandises"][i]["original_gear"]["name"] + "__**\n")
                 .WithThumbnailUrl(imageURL)
                 .AddField("**Cost** \n",
                 cost + "\n")
                 .AddField("**Main Ability** \n",
                 main + "\n")
                 .AddField("**Most Common Sub Ability Roll** \n",
                 sub + "\n")
                 .AddField("**Time Left to Buy** \n",
                 hours + " hours " + mins + " minutes " + seconds + " seconds ")
                 .WithColor(Discord.Color.DarkRed)
                 .WithCurrentTimestamp();

            return embed;

        }

        //Function used to update the buttons for the splatnet function when it is called
        public Discord.ComponentBuilder updateSplatnetButtons(int i)
        {
            var builder = new ComponentBuilder();
            dynamic net = JsonGetters.getNet();

            for (int j = 0; j < 5; j++)
            {
                if (j != i)
                    builder.WithButton(net["merchandises"][j]["original_gear"]["name"].ToString(), "splatnet" + j, Discord.ButtonStyle.Primary, null, null, false, 0);
                else
                    builder.WithButton(net["merchandises"][j]["original_gear"]["name"].ToString(), "splatnet" + j, Discord.ButtonStyle.Primary, null, null, true, 0);
            }

            return builder;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~HELPER FUNCTIONS FOR HELP FUNCTION~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        public Discord.EmbedBuilder updateHelpEmbed(int i)
        {
            var embed = new EmbedBuilder();
            string[] headText = TextArray.getHeadText();
            string[] bodyText = TextArray.getBodyText();

            //~~~~~~~~~Text and Formatting~~~~~~~~~~~~~~~~~~~~//

            embed
                .AddField(headText[i], bodyText[i])
                .WithColor(Discord.Color.Teal);

            return embed;
        }

        public Discord.ComponentBuilder updateHelpMenu(int i)
        {
            var builder = new ComponentBuilder();
            string[] headText = TextArray.getHeadText();
            string[] bodyText = TextArray.getBodyText();

            //~~~~~~~~~Text and Formatting~~~~~~~~~~~~~~~~~~~~//
            var menuBuilder = new SelectMenuBuilder()
            .WithPlaceholder("Select an option")
            .WithCustomId("menu-1")
            .WithMinValues(1)
            .WithMaxValues(1)
            .AddOption("Map Commands", "map", "Commands related to map and gamemode times")
            .AddOption("Salmon Commands", "salmon", "Commands related to Splatoon 2's Salmon Run")
            .AddOption("Splatnet Commands", "splatnet", "Commands related to accessing Splatoon 2's Splatnet shop")
            .AddOption("Miscellaneous", "misc", "Commands related to Splatoon 2 but do not fit the previous categories");

            builder
                .WithSelectMenu(menuBuilder);

            return builder;

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~RANDOM WEP~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //Function used to update the random variable; ensures that multiple Random objects need to be created
        public static int UpdateRandom(int[] indexArray) { return randomNum = rnd.Next(0, indexArray.Length); }

        //Function used to update the buttons for the Reroll option when generating a random weapon
        public Discord.EmbedBuilder UpdateRandomEmbed()
        {

            dynamic dobj = JsonGetters.getWeaponIndex(); //List of all weapons
            int[] indexArray = WeaponArray.getWeaponIndex(); //List of their indicies, because the JSON file doesn't linerally order all the numbers

            UpdateRandom(indexArray);

            var embed = new Discord.EmbedBuilder { };
            embed
                 .AddField("Your Random Weapon is the:  \n",
                  dobj["weapons"][indexArray[randomNum].ToString()]["name"] + "!")
                 .WithColor(Discord.Color.DarkMagenta);

            return embed;

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~WELCOME TEXT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //DESCRIPTION: PRODUCES INFORMATION ON THE RESOURCES THAT HELP MADE THIS BOT POSSIBLE. :) 
        //DESCRIPTION: PRODUCES INFORMATION ON A COMMAND OR LISTS ALL COMMANDS 
        public async Task userJoined(SocketGuildUser user)
        {
            await user.SendMessageAsync("**WELCOME TO THE SPLATTERBOT! **\n" +
        "This is a bot designed to help relay some information about Splatoon 2!\n" +
        "To see all the things I can do, type `$$help`\n" +
        "I do plan on implementing features for Splatoon 3 so stay tuned\n\n" +

        "If you have any questions or recommendations, feel free to send a DM to `Kukochako#3491`!\n" +
        "I'll do my best to roll out updates and fixes as fast as I can! \n" +
        "Happy Splatting!");
        }
    }


}


