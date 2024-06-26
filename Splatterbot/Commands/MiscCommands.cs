using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using Discord.Interactions.Builders;
using Newtonsoft.Json;
using Splatterbot.Functions;
using Splatterbot.LargeArrayData;
using Splatterbot;
using Discord.Interactions;

namespace Splatterbot.Commands
{
    public class MiscCommands : InteractionModuleBase //ModuleBase<CommandContext>
    {

        //Miscellaneous Commands

        //COMMAND: PING
        //DESCRIPTION: Test Command Used to return values
        [SlashCommand("ping", "pong time")]
        public async Task Ping()
        {
            await DeferAsync(); //send default response that we will edit with the actual message content

            await Context.Interaction.ModifyOriginalResponseAsync(message => { message.Content = "pong"; });

        }

        //COMMAND: RANDOMWEP
        //DESCRIPTION: PRODUCES A RANDOM WEAPON NAME. 
        [SlashCommand("randomwep2", "gives you a random Splatoon 2 weapon")]
        public async Task randomWep2()
        {
            await DeferAsync();

            //Grabbing of relavent data to randomly select from
            dynamic dobj = JsonGetters.getWeaponIndex(); //List of all weapons
            int[] indexArray = WeaponArray.getWeaponIndex(); //List of their indicies, because the JSON file doesn't linerally order all the numbers
            //Program.getRandomInt(indexArray);

            int index = Program.UpdateRandom(indexArray);

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
            var embed = new Discord.EmbedBuilder { };
            embed
                 .AddField("Your Random Weapon is the:  \n",
                  dobj["weapons"][indexArray[index].ToString()]["name"] + "!")
                 .WithColor(Discord.Color.DarkMagenta);

            var builder = new ComponentBuilder()
                .WithButton("Re-roll!", "reroll");

            await Context.Interaction.ModifyOriginalResponseAsync(message => { message.Embed = embed.Build(); message.Components = builder.Build(); });
        }

        //COMMAND: HELP
        //DESCRIPTION: PRODUCES INFORMATION ON A COMMAND OR LISTS ALL COMMANDS 
        [SlashCommand("help", "Look at information related to bot commands")]
        public async Task Help()
        {
            await DeferAsync();

            //Grabbing of relavent data to randomly select from
            string[] text = TextArray.getBodyText();

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
            var embed = new Discord.EmbedBuilder { };
            embed
                 .AddField("Select an option in the drop down menu to find the relavent tab for a function",
                 "All my functions are accessed with slash commands and are called using lowercase characters.\n" + 
                 "Splatoon 2 and 3 functions are differentiated by typing 2 or 3 after the function name respectively \n" +
                 "This textbox will update when you select an option\n")
                 .WithColor(Discord.Color.Teal);

            var menuBuilder = new SelectMenuBuilder()
                .WithPlaceholder("Select an option")
                .WithCustomId("menu-1")
                .WithMinValues(1)
                .WithMaxValues(1)
                .AddOption("Map Commands", "map", "Commands related to map and gamemode times")
                .AddOption("Salmon Commands", "salmon", "Commands related to Splatoon 2's Salmon Run")
                .AddOption("Splatnet Commands", "splatnet", "Commands related to accessing Splatoon 2's Splatnet shop")
                .AddOption("Miscellaneous", "misc", "Commands related to Splatoon 2 but do not fit the previous categories");

            var builder = new ComponentBuilder()
                .WithSelectMenu(menuBuilder);

            await Context.Interaction.ModifyOriginalResponseAsync(message => { message.Content = "\n"; message.Embed = embed.Build(); message.Components = builder.Build(); }); //("\n", components: builder.Build(), embed: embed.Build());
        }

        //COMMAND: CREDITS
        //DESCRIPTION: PRODUCES INFORMATION ON THE RESOURCES THAT HELP MADE THIS BOT POSSIBLE. :) 
        [SlashCommand("credits", "Look at information related to the creation of this bot")]
        public async Task Credits()
        {
            await DeferAsync();

            //Grabbing of relavent data to randomly select from
            string[] text = TextArray.getBodyText();

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
            var embed = new Discord.EmbedBuilder { };
            embed
                 .AddField("**__CREDITS__ **\n",
                 text[5])
                 .WithColor(Discord.Color.DarkBlue);

            await Context.Interaction.ModifyOriginalResponseAsync(message => { message.Embed = embed.Build(); });
        }
    }
}
