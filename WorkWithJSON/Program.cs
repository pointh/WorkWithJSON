using System;
using Newtonsoft.Json;

namespace WorkWithJSON
{
    public class SpaceBody
    {
        [JsonProperty("nameOfBody")]
        public string Name { get; set; }

        [JsonProperty("massOfBody")]
        public double Mass { get; set; } // * mass of Earth = mass in kg

        [JsonProperty("distanceFromSun")]
        public double Distance { get; set; } // AU = 1.495978707 . 10exp(11) m

        [JsonProperty("moon")]
        public Moon m { get; set; }

        public override string ToString()
        {
            return $"{Name}:{Mass}:{Distance}:{m?.MoonName}";
        }
    }

    public class Moon
    {
        [JsonProperty("moonName")]
        public string MoonName { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SpaceBody earth = new SpaceBody
            {
                Name = "Země",
                Mass = 1.0,
                Distance = 1.0, 
                m = new Moon { MoonName = "Měsíc"}
            };

            SpaceBody mars = new SpaceBody
            {
                Name = "Mars",
                Mass = 0.107,
                Distance = 1.3815
            };

            var earthJSON = JsonConvert.SerializeObject(earth);

            Console.WriteLine(earthJSON);

            var druhaZeme = JsonConvert.DeserializeObject<SpaceBody>(earthJSON);

            Console.WriteLine(druhaZeme);

            var marsJSON = JsonConvert.SerializeObject(mars);

            Console.WriteLine(marsJSON);

            var druhyMars = JsonConvert.DeserializeObject<SpaceBody>(marsJSON);

            Console.WriteLine(druhyMars);
            
        }
    }
}
