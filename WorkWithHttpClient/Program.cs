using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            // StringContent dědí z ByteContent a ten dědí z abstraktní HttpContent

            HttpClient htc = new HttpClient();

            try
            {
                // tohle je testovací sink pro post/get requesty
                HttpResponseMessage response = await htc.PostAsync("http://ptsv2.com/t/ssps/post", content);

                Console.WriteLine("Headers");
                foreach (KeyValuePair<string, IEnumerable<string>> v in response.Headers )
                {
                    Console.Write($"Klíč {v.Key}, Hodnota: ");
                    foreach (var t in v.Value)
                    {
                        Console.Write($"{t}");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Console.WriteLine();

                Console.WriteLine("TrailingHeaders");
                foreach (KeyValuePair<string, IEnumerable<string>> v in response.TrailingHeaders)
                {
                    Console.Write($"Klíč {v.Key}, Hodnota: ");
                    foreach (var t in v.Value)
                    {
                        Console.Write($"{t}");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine($"\nÚspěch: {response.IsSuccessStatusCode} Status: {response.StatusCode}");

                Console.WriteLine(response);
            }
            catch (HttpRequestException hre)
            {
                Console.WriteLine("Chyba: Server neslyší:" + hre);
            }

        }
    }
}
