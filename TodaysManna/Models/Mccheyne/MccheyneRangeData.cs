using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodaysManna
{
    public class MccheyneRangeData
    {
        public partial class MccheyneRangeList
        {
            [JsonProperty("ranges")]
            public List<MccheyneRange> Ranges { get; set; }
        }

        public partial class MccheyneRange
        {
            [JsonProperty("date")]
            public string Date { get; set; }

            [JsonProperty("range")]
            public string Range { get; set; }
        }
    }
}