using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithHttpClient
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
        static async Task Main(string[] args)
        {
            SpaceBody earth = new SpaceBody
            {
                Name = "Země",
                Mass = 1.0,
                Distance = 1.0,
                m = new Moon { MoonName = "Měsíc" }
            };

            var earthJSON = JsonConvert.SerializeObject(earth);

            HttpContent content = new StringContent(earthJSON, Encoding.UTF8, "application/json");

            HttpClient htc = new HttpClient();

            try
            {
                // tohle je testovací sink pro post/get requesty
                var response = await htc.PostAsync("http://ptsv2.com/t/ssps/post", content);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Console.WriteLine();
                Console.WriteLine($"Úspěch: {response.IsSuccessStatusCode} Status: {response.StatusCode}");
            }
            catch (HttpRequestException hre)
            {
                Console.WriteLine("Chyba: Server neslyší:" + hre);
            }

        }
    }
}
