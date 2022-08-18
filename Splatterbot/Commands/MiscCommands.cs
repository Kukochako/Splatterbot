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

namespace Splatterbot.Commands
{
    public class MiscCommands : ModuleBase<CommandContext>
    {

        //Miscellaneous Commands

        //COMMAND: PING
        //DESCRIPTION: Test Command Used to return values
        [Command("ping")]
        public async Task Ping()
        {

            await ReplyAsync("pong");

        }

        //COMMAND: RANDOMWEP
        //DESCRIPTION: PRODUCES A RANDOM WEAPON NAME. 
        //PARAMETERS: $$RANDOMWEP [WEAPON TYPE]
        [Command("randomwep")]
        public async Task randomWep()
        {
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

            await Context.Channel.SendMessageAsync(embed: embed.Build(), components: builder.Build());
        }

        //COMMAND: HELP
        //DESCRIPTION: PRODUCES INFORMATION ON A COMMAND OR LISTS ALL COMMANDS 
        //PARAMETERS: $$HELP [COMMAND]
        [Command("help")]
        public async Task Help()
        {
            //Grabbing of relavent data to randomly select from
            string[] text = TextArray.getBodyText();

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
            var embed = new Discord.EmbedBuilder { };
            embed
                 .AddField("Select an option in the drop down menu to find the relavent tab for a function",
                 "All my functions start with the prefix '$$' and are called using lowercase characters.\n" + "This textbox will update when you select an option\n")
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

            await Context.Channel.SendMessageAsync("\n", components: builder.Build(), embed: embed.Build());
        }

        //COMMAND: CREDITS
        //DESCRIPTION: PRODUCES INFORMATION ON THE RESOURCES THAT HELP MADE THIS BOT POSSIBLE. :) 
        //DESCRIPTION: PRODUCES INFORMATION ON A COMMAND OR LISTS ALL COMMANDS 
        //PARAMETERS: $$HELP [COMMAND]
        [Command("credits")]
        public async Task Credits()
        {
            //Grabbing of relavent data to randomly select from
            string[] text = TextArray.getBodyText();

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
            var embed = new Discord.EmbedBuilder { };
            embed
                 .AddField("**__CREDITS__ **\n",
                 text[5])
                 .WithColor(Discord.Color.DarkBlue);

            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }
    }
}
