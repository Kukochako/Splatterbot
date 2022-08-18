using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace Splatterbot.Functions
{
    public class JsonGetters
    {

        static dynamic mapdobj = null; //JSON data for basic map info
        static dynamic salmondobj = null; //JSON data for salmon run info
        static dynamic netdobj = null; //JSON data for splatnet info

        static dynamic weapondobj = null; //JSON data for weapon info

        static WebClient cl = new WebClient(); // Object used to access the internet

        public static dynamic getMap()
        {
            //This if statement prevents from unneccisarily refreshing the JSON file to reduce online traffic and improve run time
            if (mapdobj == null || DateTimeOffset.Now.ToUnixTimeSeconds() > (double)mapdobj["regular"][0]["end_time"])
            { //Refresh json data if the variable is empty or the two hours have elapsed since last updating
                string s2info = cl.DownloadString("https://splatoon2.ink/data/schedules.json"); // Grabs all the data from the link to the JSON file            

                mapdobj = JsonConvert.DeserializeObject<dynamic>(s2info); // Deserializes the downloaded JSON info so information can readily be read from it
            }

            return mapdobj;

        }

        public static dynamic getSalmon()
        {

            //This if statement prevents from unneccisarily refreshing the JSON file to reduce online traffic and improve run time
            if (salmondobj == null || DateTimeOffset.Now.ToUnixTimeSeconds() > (double)salmondobj["details"][0]["end_time"])
            { //Refresh json data if the variable is empty or the two hours have elapsed since last updating
                string s2info = cl.DownloadString("https://splatoon2.ink/data/coop-schedules.json"); // Grabs all the data from the link to the JSON file            

                salmondobj = JsonConvert.DeserializeObject<dynamic>(s2info); // Deserializes the downloaded JSON info so information can readily be read from it
            }
            return salmondobj;
        }

        public static dynamic getNet()
        {

            //This if statement prevents from unneccisarily refreshing the JSON file to reduce online traffic and improve run time
            if (netdobj == null || DateTimeOffset.Now.ToUnixTimeSeconds() > (double)netdobj["merchandises"][0]["end_time"])
            { //Refresh json data if the variable is empty or the two hours have elapsed since last updating
                string s2info = cl.DownloadString("https://splatoon2.ink/data/merchandises.json"); // Grabs all the data from the link to the JSON file            

                netdobj = JsonConvert.DeserializeObject<dynamic>(s2info); // Deserializes the downloaded JSON info so information can readily be read from it
            }
            return netdobj;
        }

        public static dynamic getWeaponIndex()
        {

            //No real way to check if weapon list has been updated
            if (weapondobj == null)
            { //Refresh json data if the variable is empty or the two hours have elapsed since last updating
                string s2info = cl.DownloadString("https://splatoon2.ink/data/locale/en.json"); // Grabs all the data from the link to the JSON file            

                weapondobj = JsonConvert.DeserializeObject<dynamic>(s2info); // Deserializes the downloaded JSON info so information can readily be read from it
            }
            return weapondobj;

        }

    }
}
