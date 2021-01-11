using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodaysManna
{
    public class MannaData
    {
        [JsonProperty("verse")]
        public string Verse { get; set; }

        [JsonProperty("contents")]
        public string[] Contents { get; set; }

        public List<(int, string)> Content = new List<(int, string)>();

        public string TodayString { get; set; }

        public string AllString { get; set; }
    }
}
