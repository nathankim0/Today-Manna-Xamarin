using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodaysManna
{
    public class BibleAtData
    {
        public partial class BibleList
        {
            [JsonProperty("bibles")]
            public List<Bible> Bibles { get; set; }
        }

        public partial class Bible
        {
            [JsonProperty("kor")]
            public string Kor { get; set; }

            [JsonProperty("eng")]
            public string Eng { get; set; }
        }
    }
}
