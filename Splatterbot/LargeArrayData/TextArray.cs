using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splatterbot.LargeArrayData
{
    class TextArray
    {

        static string[] headText = {
        //Strings 0-4 are help Function info
        //String 5th string is the credits message
        //Last String is Welcome message

        //[0] Help Function Default Text
        "Select the option in the drop down menu to find the relavent tab for your function:\n",

        //[1] Map Related
        "**__MAP RELATED COMMANDS__**\n",

        //[2] Salmon Related
        "**__SALMON RUN RELATED COMMANDS__**\n",

        //[3] Splatnet Related
        "**__SPLATNET RELATED COMMANDS__**\n",

        //[4] Miscellaneous
        "**__MISC COMMANDS__**\n",

        //-------------------------------------------------------------------------------------------------------------------------//

        //[5] Credits
        "**__CREDITS__**\n",
        
        //-------------------------------------------------------------------------------------------------------------------------//

        //[6] Welcome Message:
        "**WELCOME TO THE SPLATTERBOT!**\n",
        };

        static string[] bodyText = {
        //Strings 0-4 are help Function info
        //String 5th string is the credits message
        //Last String is Welcome message

        //[0] Help Function Default Text
        "All functions are called using lowercase characters.\n" +
        "Select the option in the drop down menu to find the relavent tab for your function:\n",

        //[1] Map Related
        //"**__MAP RELATED COMMANDS__**\n" +
        "\n**turf**\n" +
        "Posts Current maps available in turf war as well as the maps in the next rotation\n\n"+
        "**ranked**\n" +
        "Posts Current maps and gamemode available in ranked battle as well as the maps and gamemode in the next rotation\n\n"+
        "**league**\n" +
        "Posts Current maps and gamemode available in league battle as well as the maps and gamemode in the next rotation\n\n"+
        "**time**\n" +
        "Posts time left before next rotation\n\n" +
        "**maps**\n" +
        "Posts Current maps and gamemode available in turf, ranked, and league battle as well as the maps, gamemode, and time before next rotation",

        //[2] Salmon Related
        //"**__SALMON RUN RELATED COMMANDS__**\n" +
        "\n**salmon**\n" +
        "Posts current map and weapons available in the current or upcoming run as well as how long until the end or start of the run respectively\n\n" +
        "**gamemodes**\n" +
        "Posts all relavent information about the current and upcoming maps and gamemodes, but also includes information on salmon run",

        //[3] Splatnet Related
        //"**__SPLATNET RELATED COMMANDS__**\n" +
        "\n**splatnet**\n" +
        "Displays current items available in the Splatnet shop and info related to main ability, cost, and common roll for the sub ability\n\n",

        //[4] Miscellaneous
        //"**__MISC COMMANDS__**\n" +
        "\n**randomwep**\n" +
        "Produces a randomly selected weapon\n\n" +
        "**help**\n" +
        "Produces a menu that describes all relevant info regarding the functions this bot can perform\n\n" +
        "**credits**\n" +
        "Produces information that explains all the work that made this bot possible\n\n" +
        "**whenis <map name or gamemode> **\n" +
        "Posts when the next time a map or gamemode will appear in the turf, ranked, or league battle\n" +
        "This function does NOT check if the gamemode is currently available, rather just when the next occurance is\n" +
        "Additionally, case sensitivity does not matter; however proper spelling is required\n" +
        "__EXAMPLE:__ $$whenis new albacore hotel",

        //-------------------------------------------------------------------------------------------------------------------------//

        //[5] Credits
        //"**__CREDITS__**\n" +
        "Hello, and thank you for trying Splatterbot!\n\n" +
        "This bot is a FREE project that I started on my own just for fun and out of convenience for my friends to use\n" +
        "While this bot is coded on my own, I pull from a variety of resources to help create this:\n" +
        "\t**Splatoon2.ink** - This website is where I pull most of my information regarding map info for. You can find the database at https://github.com/misenhower/splatoon2.ink \n" +
        "\t-All images used in my bot are from Imgur (https://imgur.com/) \n\n" +
        "If you need to message me for any reason, feel free to send a DM to `SpaceLechuga` on Discord!\n" +
        "This bot was coded using Discord.Net",
        
        //-------------------------------------------------------------------------------------------------------------------------//

        //[6] Welcome Message:
        "**WELCOME TO THE SPLATTERBOT!**\n" +
        "This is a bot designed to help relay some information about Splatoon 2!\n" +
        "To see all the things I can do, type `$$help`\n" +
        "I do plan on implementing features for Splatoon 3 so stay tuned\n\n" +

        "If you have any questions or recommendations, feel free to send a DM to `SpaceLechuga` on Discord!\n" +
        "I'll do my best to roll out updates and fixes as fast as I can! \n" +
        "Happy Splatting!"

        };

        //returns the Header text array
        public static string[] getHeadText() { return headText; }

        //returns the body text array
        public static string[] getBodyText() { return bodyText; }
    }
}
