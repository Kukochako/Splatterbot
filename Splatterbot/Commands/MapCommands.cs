using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json;
using Splatterbot.Functions;

namespace Splatterbot.Commands
{
    //This Class is used to determine the different actions that will be taken depending on the users input message
    public class MapCommands : ModuleBase<CommandContext>
    {

        string imagePrefix = "https://splatoon2.ink/assets/splatnet"; //contains the prefix for the image links stored in the JSON files

        //Map Related Commands
        //Produce Information on the maps and gamemodes currently Active

        //COMMAND: TIME
        //DESCRIPTION: Prints out the time left in the current rotation
        [Command("time")]
        public async Task TimeLeft()
        {

            dynamic dobj = JsonGetters.getMap();

            //Time Until Next Rotation
            //Calculated based off of current time and what time the funciton was called
            var unixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds(); //((double)((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().TotalSeconds - dobj["modes"]["gachi"][0]["startTime"]); //floor($seconds/3600)
            double timeLeft = (double)dobj["regular"][0]["end_time"] - (double)unixTimestamp;

            double hours = Math.Floor(timeLeft / 3600);
            double mins = Math.Floor((timeLeft / 60)) - (60 * hours);
            double seconds = (timeLeft % 60);

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

            //Embeds are ways to print the text form the bot with the ability to change the color of the side tabs on the messages
            var embed = new Discord.EmbedBuilder { };
            embed.AddField("**__Time Left Before Next Rotation:__**\n",
                 hours + " hours " + mins + " minutes " + seconds + " seconds ")
                .WithColor(Discord.Color.Purple)
                .WithCurrentTimestamp();



            await ReplyAsync(embed: embed.Build()); //build the embed then send when ready
        }

        //COMMAND: TURF
        //DESCRIPTION: Prints out current and upcoming maps realted to Turf War 
        [Command("turf")]
        public async Task Turf()
        {
            dynamic dobj = JsonGetters.getMap();

            //Turf War Info
            //Maps
            string tmap1 = dobj["regular"][0]["stage_a"]["name"].ToString() + ", " + dobj["regular"][0]["stage_b"]["name"].ToString(); // Current
            string tmap2 = dobj["regular"][1]["stage_a"]["name"].ToString() + ", " + dobj["regular"][1]["stage_b"]["name"].ToString(); // Next

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

            var embed = new Discord.EmbedBuilder { };
            embed
                .WithTitle("**__Turf War Schedule:__**\n")
                .WithThumbnailUrl("https://i.imgur.com/iLA9zaN.png")
                .AddField("**Current Maps** \n",
                tmap1 + "\n")
                .AddField("**Next Maps** \n",
                tmap2 + "\n")
                .WithColor(Discord.Color.Green);

            await ReplyAsync(embed: embed.Build()); //build the embed then send when ready

        }

        //COMMAND: RANKED
        //DESCRIPTION: Prints out current and upcoming maps as well as game types for Ranked Battle 
        [Command("ranked")]
        public async Task Ranked()
        {
            dynamic dobj = JsonGetters.getMap();

            //Ranked Battle Info
            //Maps
            string rmap1 = dobj["gachi"][0]["stage_a"]["name"].ToString() + ", " + dobj["gachi"][0]["stage_b"]["name"].ToString(); // Current
            string rmap2 = dobj["gachi"][1]["stage_a"]["name"].ToString() + ", " + dobj["gachi"][1]["stage_b"]["name"].ToString(); // Next

            //Gamemode
            string rgame1 = dobj["gachi"][0]["rule"]["name"]; // Current
            string rgame2 = dobj["gachi"][1]["rule"]["name"]; // Next

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

            var embed = new Discord.EmbedBuilder { };
            embed
                .WithTitle("**__Ranked Battle Schedule:__**\n")
                .WithThumbnailUrl("https://i.imgur.com/PzitKch.png")
                .AddField("**Current Gamemode** \n",
                rgame1 + "\n")
                .AddField("**Current Maps** \n",
                rmap1 + "\n")
                .AddField("**Next Gamemode** \n",
                rgame2 + "\n")
                .AddField("**Next Maps** \n",
                rmap2 + "\n")
                .WithColor(Discord.Color.DarkOrange);

            await ReplyAsync(embed: embed.Build()); //build the embed then send when ready

        }

        //COMMAND: LEAGUE
        //DESCRIPTION: Prints out current and upcoming maps as well as game types for League Battle 
        [Command("league")]
        public async Task League()
        {
            dynamic dobj = JsonGetters.getMap();

            string URL1 = imagePrefix + dobj["league"][0]["stage_a"]["image"];
            string URL2 = imagePrefix + dobj["league"][0]["stage_b"]["image"];

            //League Battle 
            //Maps
            string lmap1 = dobj["league"][0]["stage_a"]["name"] + ", " + dobj["league"][0]["stage_b"]["name"]; // Current
            string lmap2 = dobj["league"][1]["stage_a"]["name"] + ", " + dobj["league"][1]["stage_b"]["name"]; // Next

            //Gamemode
            string lgame1 = dobj["league"][0]["rule"]["name"]; // Current
            string lgame2 = dobj["league"][1]["rule"]["name"]; // Next

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

            var embed = new Discord.EmbedBuilder { };
            embed
                .WithTitle("**__League Battle Schedule:__**\n")
                .WithThumbnailUrl("https://i.imgur.com/S9EVCN5.png")
                .AddField("**Current Gamemode** \n",
                lgame1 + "\n")
                .AddField("**Current Maps** \n",
                lmap1 + "\n")
                .AddField("**Next Gamemode** \n",
                lgame2 + "\n")
                .AddField("**Next Maps** \n",
                lmap2 + "\n")
                .WithColor(Discord.Color.DarkMagenta);

            await ReplyAsync(embed: embed.Build()); //build the embed then send when ready

        }

        //COMMAND: SALMON
        //DESCRIPTION: Prints out current and upcoming maps as well as weapons for Salmon Run as well as available weapons
        [Command("salmon")]
        public async Task Salmon()
        {
            dynamic dobj = JsonGetters.getSalmon();

            string map = dobj["details"][0]["stage"]["name"].ToString();
            string imageURL = imagePrefix + dobj["details"][0]["stage"]["image"];
            string remainingText, activeText; //These strings will determine what text will appear in the embed depending if the current run is active or not


            //Find the next run listed in the JSON that hasn't already ended
            int i = 0; //Control variable that is used to determine which run is the closest and hasn't elapsed already
            while ((double)dobj["details"][i]["end_time"] < (double)DateTimeOffset.Now.ToUnixTimeSeconds()) { i++; }

            //Calculates on the assumption that the run is active, if the resulting number is positive, then the earlierst available run is not active
            double timeLeft;
            //double temp = (double)dobj["details"][i]["start_time"] - (double)DateTimeOffset.Now.ToUnixTimeSeconds();

            if (((double)dobj["details"][i]["start_time"] < ((double)DateTimeOffset.Now.ToUnixTimeSeconds()) && (double)dobj["details"][0]["end_time"] > (double)DateTimeOffset.Now.ToUnixTimeSeconds()))
            { //if current time is inbetween start and end times, then the run is active

                remainingText = "**Time Left in Shift** \n";
                activeText = "**ACTIVE**! \n";
                timeLeft = (double)dobj["details"][i]["end_time"] - (double)DateTimeOffset.Now.ToUnixTimeSeconds();

            }
            else
            { //Text if the closest run is inactive

                remainingText = "**Time Left Until the Start of the Next Shift** \n";
                activeText = "**NOT ACTIVE**! \n";
                timeLeft = (double)dobj["details"][i]["start_time"] - (double)DateTimeOffset.Now.ToUnixTimeSeconds();

            }

            //double timeLeft = (double)dobj["details"][0][timeText.ToString()] - (double)DateTimeOffset.Now.ToUnixTimeSeconds();

            double hours = Math.Floor(timeLeft / 3600);
            double mins = Math.Floor((timeLeft / 60)) - (60 * hours);
            double seconds = (timeLeft % 60);

            //Weapons: dobj["details"][0]["weapons"][0]*["weapon"]["name"]

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

            string[] weapons = new string[4]; //String that will hold the path for the names of the weapon for the current salmon run
            for (i = 0; i < 4; i++)
            {
                if (dobj["details"][0]["weapons"][i]["id"].ToString().Equals("-1")) //Checks if the weapon for this slot is a special weapon
                    weapons[i] = dobj["details"][0]["weapons"][i]["coop_special_weapon"]["name"];
                else
                    weapons[i] = dobj["details"][0]["weapons"][i]["name"]["name"];
            }

            var embed = new Discord.EmbedBuilder { };
            embed
                .WithTitle("**__Salmon Run Schedule:__**\n")
                .WithThumbnailUrl("https://cdn.wikimg.net/en/splatoonwiki/images/1/15/S2_Brand_Grizzco.png")
                .AddField("This run is currently ", activeText) //+ " " + temp)
                .AddField("**Current Map** \n",
                map + "\n")
                .AddField("**Weapons** \n",
                weapons[0] + ", " +
                weapons[1] + ", " +
                weapons[2] + ", " +
                weapons[3] + "\n")
                .AddField(remainingText,
                hours + " hours " + mins + " minutes " + seconds + " seconds ")
                .WithColor(Discord.Color.Orange);

            await ReplyAsync(embed: embed.Build()); //build the embed then send when ready                
        }

        //COMMAND: MAPS
        //DESCRIPTION: Prints out current and upcoming maps 
        [Command("maps")]
        public async Task Maps()
        {

            //Calls all the corresponding functions
            await Turf();
            await Ranked();
            await League();
            await TimeLeft();

        }

        //COMMAND: GAMEMODES
        //DESCRIPTION: Prints out current and upcoming maps as well as game types for all relavent gamemodes and time left unil the start of next
        [Command("gamemodes")]
        public async Task Gamemodes()
        {

            //Calls all the corresponding functions
            await Turf();
            await Ranked();
            await League();
            await TimeLeft();
            await Salmon();

        }

        //COMMAND: WHENIS
        //DESCRIPTION: Returns when the next occurrance of a map or gamemode is 
        [Command("whenis")]
        public async Task whenIs([Remainder] string stdin)
        {

            dynamic dobj = JsonGetters.getMap(); //Grabs relavent map data

            bool isMap = true; //Used to determine wheter or not a map or gamemode is being searched for
            string key = stdin; //Moves user input into a temp string that can be modified
            string noText = stdin + " does not seem to appear anytime soon...";

            double[] times = { 0, 0, 0 }; //Containes the times when the desired gamemodes/maps occur
                                          //0 - Will refer to turf war
                                          //1 - Will refer to ranked battle
                                          //2 - Will refer to league battle

            //Removes all the space characters and converts all the characters to lowercase to make name more uniform
            key = key.Replace(" ", "").ToLower();

            //First determine whether or not the user has searched for a map
            if (key.Equals("towercontrol") || key.Equals("splatzones") || key.Equals("clamblitz") || key.Equals("rainmaker")) { isMap = false; }

            //Iterate through the list of maps until the relavant map is found or all known upcoming gamemodes have been searched
            for (int i = 1; i < 12; i++)
            {
                //Iterate through these sections if we are searching for maps
                if (isMap)
                {

                    if (times[0] == 0)
                    { //If the map has yet to be found in turf war mode
                      //Search stage_a and stage_b
                        if (key.Equals(dobj["regular"][i]["stage_a"]["name"].ToString().Replace(" ", "").ToLower()) || key.Equals(dobj["regular"][i]["stage_b"]["name"].ToString().Replace(" ", "").ToLower()))
                            times[0] = (double)dobj["regular"][i]["start_time"];

                    }

                    if (times[1] == 0)
                    { //If the map has yet to be found in ranked mode
                      //Search stage_a and stage_b
                        if (key.Equals(dobj["gachi"][i]["stage_a"]["name"].ToString().Replace(" ", "").ToLower()) || key.Equals(dobj["gachi"][i]["stage_b"]["name"].ToString().Replace(" ", "").ToLower()))
                            times[1] = (double)dobj["gachi"][i]["start_time"];
                    }

                    if (times[2] == 0)
                    { //If the map has yet to be found in league mode
                      //Search stage_a and stage_b
                        if (key.Equals(dobj["league"][i]["stage_a"]["name"].ToString().Replace(" ", "").ToLower()) || key.Equals(dobj["league"][i]["stage_b"]["name"].ToString().Replace(" ", "").ToLower()))
                            times[2] = (double)dobj["league"][i]["start_time"];
                    }

                }
                //Iterate through these sections if we are looking for gamemodes
                else
                {
                    //await ReplyAsync(key); //build the embed then send when ready    
                    if (times[1] == 0)
                    { //If the gamemode has yet to be found in ranked mode                        
                      //Search the gamemode
                        if (key.Equals(dobj["gachi"][i]["rule"]["name"].ToString().Replace(" ", "").ToLower()))
                            times[1] = (double)dobj["gachi"][i]["start_time"];
                    }

                    if (times[2] == 0)
                    { //If the gamemode has yet to be found in league mode                        
                      //Search the gamemode
                        if (key.Equals(dobj["league"][i]["rule"]["name"].ToString().Replace(" ", "").ToLower()))
                            times[2] = (double)dobj["league"][i]["start_time"];
                    }

                }
            }

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~TEXT FORMATTING AND PRINTING~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//    

            //Decide what will be printed if the requested map/gamemode does not have any appearances coming up

            if (times[0] == 0 && times[1] == 0 && times[2] == 0)
            { //No time is found
                await Context.Channel.SendMessageAsync("Can't find any times for " + stdin + " in the near future :(");
            }
            else
            {

                //These arrays contain the properly converted Unix times
                //0 - Will refer to hours
                //1 - Will refer to minutes
                //2 - Will refer to seconds
                double[] realTime1 = new double[3];
                double[] realTime2 = new double[3];
                double[] realTime3 = new double[3];

                if (isMap)
                { //Embed Format for Map Search

                    var embed = new Discord.EmbedBuilder { };
                    string temp1 = noText; string temp2 = noText; string temp3 = noText; //These text will determine what shows for each available game time

                    if (times[0] != 0) { realTime1 = convertTime(times[0]); temp1 = realTime1[0] + " hours " + realTime1[1] + " minutes " + realTime1[2] + " seconds "; }
                    if (times[1] != 0) { realTime2 = convertTime(times[1]); temp2 = realTime2[0] + " hours " + realTime2[1] + " minutes " + realTime2[2] + " seconds "; }
                    if (times[2] != 0) { realTime3 = convertTime(times[2]); temp3 = realTime3[0] + " hours " + realTime3[1] + " minutes " + realTime3[2] + " seconds "; }

                    embed.WithTitle("**__Time until the next appearance of " + stdin + " in:__**\n")
                        .AddField("**Turf War**",
                        temp1) //+ " " + temp)
                        .AddField("**Ranked Battle** \n",
                        temp2 + "\n")
                        .AddField("**League Battle** \n",
                        temp3 + "\n")
                        .WithColor(Discord.Color.Teal);

                    await ReplyAsync(embed: embed.Build()); //build the embed then send when ready                

                }
                else
                { //Embed Format for Gamemode Search

                    var embed = new Discord.EmbedBuilder { };
                    string temp2 = noText; string temp3 = noText; //These text will determine what shows for each available game time

                    if (times[1] != 0) { realTime2 = convertTime(times[1]); temp2 = realTime2[0] + " hours " + realTime2[1] + " minutes " + realTime2[2] + " seconds "; }
                    if (times[2] != 0) { realTime3 = convertTime(times[2]); temp3 = realTime3[0] + " hours " + realTime3[1] + " minutes " + realTime3[2] + " seconds "; }

                    embed.WithTitle("**__Time until the next appearance of " + stdin + " in:__**\n")
                        .AddField("**Ranked Battle** \n",
                        temp2 + "\n")
                        .AddField("**League Battle** \n",
                        temp3 + "\n")
                        .WithColor(Discord.Color.Teal);

                    await ReplyAsync(embed: embed.Build()); //build the embed then send when ready     

                }
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~HELPER FUNCTIONS FOR WHENIS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        //Converts the Unix time to a readable format
        public static double[] convertTime(double timeLeft)
        {

            double[] returnMe = new double[3];

            timeLeft -= (double)DateTimeOffset.Now.ToUnixTimeSeconds();

            double hours = Math.Floor(timeLeft / 3600);
            double mins = Math.Floor((timeLeft / 60)) - (60 * hours);
            double seconds = (timeLeft % 60);

            returnMe[0] = hours;
            returnMe[1] = mins;
            returnMe[2] = seconds;

            return returnMe;
        }

    }
}
