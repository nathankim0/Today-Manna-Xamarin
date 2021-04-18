using Newtonsoft.Json;

namespace TodaysManna.Models
{
    public class JsonMannaModel
    {
        [JsonProperty("verse")]
        public string Verse { get; set; } = "";

        [JsonProperty("contents")]
        public string[] Contents { get; set; } = { "", "" };
    }
}
