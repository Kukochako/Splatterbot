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

namespace Splatterbot.LargeArrayData
{
    class WeaponArray : ModuleBase<CommandContext>
    {

        static int[] weapons = { 0, 1, 2, 10, 12, 20, 21, 30, 32, 40, 42, 50, 51, 52, 60, 61, 62, 70, 72, 80, 81, 90, 200, 201, 202, 210, 220, 221, 222, 230, 231, 240, 241, 242, 250, 251, 300, 301, 302, 310, 311, 312, 400, 401, 1000, 1001, 1010, 1012, 1020, 1022, 1030, 1100, 1102, 1110, 1111, 1112, 2000, 2001, 2002, 2010, 2012, 2020, 2022, 2030, 2040, 2050, 2051, 2052, 2060, 2061, 3000, 3001, 3002, 3010, 3011, 3020, 3021, 3022, 3030, 3031, 3040, 3041, 4000, 4001, 4002, 4010, 4012, 4020, 4021, 4030, 4031, 4040, 4041, 5000, 5001, 5002, 5010, 5012, 5020, 5021, 5022, 5030, 5031, 5040, 5041, 6000, 6001, 6010, 6011, 6012, 6020, 6021, 6022 };

        //returns the weapon array with the indexes for the weapons listed in the Splatoon 2 JSON
        public static int[] getWeaponIndex() { return weapons; }

    }
}
