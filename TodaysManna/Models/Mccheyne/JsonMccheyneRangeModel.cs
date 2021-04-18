using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodaysManna.Models
{
    public class JsonMccheyneRangeModel
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