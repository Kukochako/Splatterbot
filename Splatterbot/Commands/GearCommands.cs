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

namespace Splatterbot.Commands
{

    //Implementation of IEmote interface allows for the use of functions related to emojis
    public interface IEmote { }

    public class GearCommands : ModuleBase<CommandContext>, IEmote
    {
        //JSON data for gear related commands
        dynamic splatnetDobj = JsonGetters.getNet();

        string imagePrefix = "https://splatoon2.ink/assets/splatnet"; //contains the prefix for the image links stored in the JSON files

        //Gear Related Commands
        //Produces Information related to what gear is currently available with their stats and prices

        //Nintendo Switch Online Store
        [Command("splatnet")]
        public async Task splatnet()
        {

            dynamic net = JsonGetters.getNet();
            //Implements a feature where text will change depending on a given emote allowing the bot to essentially create a scrollable list

            double timeLeft = (double)net["merchandises"][0]["end_time"] - (double)DateTimeOffset.Now.ToUnixTimeSeconds();

            double hours = Math.Floor(timeLeft / 3600);
            double mins = Math.Floor((timeLeft / 60)) - (60 * hours);
            double seconds = (timeLeft % 60);

            string imageURL = imagePrefix + net["merchandises"][0]["gear"]["image"];
            string cost = net["merchandises"][0]["price"] + "\t\t";
            string main = net["merchandises"][0]["skill"]["name"] + "\t\t";
            string sub = net["merchandises"][0]["gear"]["brand"]["frequent_skill"]["name"] + "\t\t";

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

            //Sets up default options for when menu is first created
            var embed = new Discord.EmbedBuilder { };
            embed
                 .WithTitle("**__" + net["merchandises"][0]["original_gear"]["name"] + "__**\n")
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

            var builder = new ComponentBuilder();
            for (int j = 0; j < 5; j++)
            {
                if (j != 0)
                    builder.WithButton(net["merchandises"][j]["original_gear"]["name"].ToString(), "splatnet" + j, Discord.ButtonStyle.Primary, null, null, false, 0);
                else
                    builder.WithButton(net["merchandises"][j]["original_gear"]["name"].ToString(), "splatnet" + j, Discord.ButtonStyle.Primary, null, null, true, 0);
            }

            await Context.Channel.SendMessageAsync(embed: embed.Build(), components: builder.Build()); //channel.SendMessageAsync(embed: embed.Build());//ReplyAsync(embed: embed.Build()); //build the embed then send when 

        }


    }

}
