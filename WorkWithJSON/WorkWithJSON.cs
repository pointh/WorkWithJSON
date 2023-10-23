using System;
using System.Linq;
using System.Collections.Generic;

// Newtonsoft.Json (https://www.newtonsoft.com/json) je nejpoužívanější knihovnou pro převod třídy do JSON a naopak
// Není součástí .NET, ale je volně dostupná jako rozšiřující balíček Nuget
// Instalace: viz video na virtuální škole a v Teamsech
// 
// Používá atributy u vlastností. Atributy popisují, jak bude vypadat reprezentace vlastnosti
// v JSON - přídávají k vlastnosti identifikátor(jméno v JSON).

// Pokud nepřidáme atributy k vlastnostem, budou se v JSON objevovat s identifikátorem, který je stejný jako jméno
// vlastnosti.
using Newtonsoft.Json;

namespace WorkWithJSON
{

    public class SpaceBody
    {
        // Kdybychom nepoužili atribut JsonProperty, Newtonsoft by pro pojmenování
        // elementů v  JSON použil názvy vlastností (Name, Mass, atd.)
        // To většinou nechceme nebo nemůžeme použít, protože 
        // např. využíváme API, kde jména definoval ten, kdo API poskytuje.

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

 /* Takto bude vypadat převod z objektu typu SpaceBody na JSON:
  * 
  * {"nameOfBody":"Země","massOfBody":1.0,"distanceFromSun":1.0,"moon":{"moonName":"Měsíc"}}
  * 
  * 1. IsInhabited se nebude převádět, protože má atribut JsonIgnore
  * 2. Argument v JsonProperty se použije pro pojmenování JSON elementu
  * 3. Vnořené objekty v C# mají svou obdobu ve vnořených objektech v JSON:
  *    SpaceBody{Moon} odpovídá v JSON {"moon":{"moonName":"Měsíc"}}
  *    
  *  Převod opačným směrem používá názvy elementů pro inicializaci objektu a provádí datové
  *  konverze podle typu vlastností.
  */

    class WorkWithJSON
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

            var druhaZeme = JsonConvert.DeserializeObject<SpaceBody>(earthJSON);

            var marsJSON = JsonConvert.SerializeObject(mars);

            var druhyMars = JsonConvert.DeserializeObject<SpaceBody>(marsJSON);

            // Merkur nemá měsíc
            string mercuryJSON = "{\"nameOfBody\":\"Merkur\",\"massOfBody\":0.056,\"distanceFromSun\":0.4}";
            var mercury = JsonConvert.DeserializeObject<SpaceBody>(mercuryJSON);
            /*
            * Je stejné jako SpaceBody mercury = new SpaceBody
            * {
            *      Name = "Merkur",
            *      Mass = 0.056,
            *      Distance = 0.4,
            *      m = null,
            *      IsInhabited = false
            *  }
            *  
            *  Při převodu z JSON do objektu musíme v generické metodě uvést, do jakého typu chceme JSON převádět (<SpaceBody>)
            */

            List<SpaceBody> planets = new List<SpaceBody> { earth, mars, druhaZeme, druhyMars, mercury };

            foreach (var x in planets)
                Console.WriteLine(x);

            var planetsJSON = JsonConvert.SerializeObject(planets);

            // Na výstupu je vidět, jak se v JSON realizuje List ...
            Console.WriteLine("\n" + planetsJSON + "\n");

            // A zcela intuitivně funguje i konverze směrem zpět
            // V tomto případě vytváříme objekt typu List<SpaceBody>, který se musí
            // objevit ve volání DeserializeObject
            List<SpaceBody> anotherPlanets = JsonConvert.DeserializeObject<List<SpaceBody>>(planetsJSON);

            foreach (var x in anotherPlanets)
                Console.WriteLine(x);
        }
    }
}
