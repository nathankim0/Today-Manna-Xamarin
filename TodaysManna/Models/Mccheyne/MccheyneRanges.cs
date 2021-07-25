using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodaysManna.Models
{
    public class MccheyneRanges
    {
        [JsonProperty("ranges")]
        public List<MccheyneRange> Ranges { get; set; }
    }
}
