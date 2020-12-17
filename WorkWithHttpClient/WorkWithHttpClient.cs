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

        [JsonIgnore]
        public bool IsInhabited { get; set; }

        public override string ToString()
        {
            return $"{Name}:{Mass}:{Distance}:{m?.MoonName}:{IsInhabited}";
        }
    }

    public class Moon
    {
        [JsonProperty("moonName")]
        public string MoonName { get; set; }
    }


    class WorkWithHttpClient
    {
        // async ještě probereme, teď ho berte tak jak je.
        static async Task Main(string[] args)
        {
            SpaceBody earth = new SpaceBody
            {
                Name = "Země",
                Mass = 1.0,
                Distance = 1.0,
                m = new Moon { MoonName = "Měsíc" },
                IsInhabited = false
            };

            var earthJSON = JsonConvert.SerializeObject(earth);

            
            HttpContent content = new StringContent(earthJSON, Encoding.UTF8, "application/json");
            // StringContent dědí z ByteContent a ten dědí z abstraktní HttpContent

            // Toto je klient, který funguje stejně jako náš browser, jen je s ním těžší pořízení
            HttpClient htc = new HttpClient();

            try
            {
                // tohle je testovací sink pro post/get requesty
                // Sink ssps jsem vytvořil před týdnem, pokud ho smazali, vytvořte si "New Random Toilet"
                // a nově vytvořenou adresu vložte do následujícího odkazu.
                // Na adrese, ve které vynecháte /post (http://ptsv2.com/t/cqzh9-1608204865) uvidíte obsah toho, 
                // co jsete poslali na server

                // HttpResponseMessage není tak jednoduchá, jak by se mohlo zdát
                // breakpoint za voláním PostAsync ukáže, co všechno se přenáší mezi klientem a serverem ...

                // await pošle úlohu do thread pool vlákna. V něm zavolá metodu PostAsync a pošle content
                // na server na uvedené adrese
                HttpResponseMessage response = await htc.PostAsync("http://ptsv2.com/t/cqzh9-1608204865/post", content);

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

                // Content může být rozsáhlý, proto pokud nechceme blokovat vlákno uživatelského rozhraní,
                // provedeme načtení na thread pool
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
            // když něco selže ...
            catch (HttpRequestException hre)
            {
                Console.WriteLine("Chyba: Server neslyší:" + hre);
            }

        }
    }
}
