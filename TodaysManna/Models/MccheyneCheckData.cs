using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace TodaysManna.Models
{
    public class MccheyneCheckData
    {
        public partial class MccheyneCheckRangeList
        {
            [JsonProperty("ranges")]
            public List<MccheyneCheckRange> CheckRanges { get; set; }
        }

        public partial class MccheyneCheckRange
        {
            [JsonProperty("date")]
            public string Date { get; set; }

            [JsonProperty("range1")]
            public string Range1 { get; set; }

            [JsonProperty("range2")]
            public string Range2 { get; set; }

            [JsonProperty("range3")]
            public string Range3 { get; set; }

            [JsonProperty("range4")]
            public string Range4 { get; set; }

            [JsonProperty("range5")]
            public string Range5 { get; set; } = "";
        }
    }
}
