using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodaysManna
{
    public partial class BookResult
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("book_name")]
        public string BookName { get; set; }

        [JsonProperty("book_short")]
        public string BookShort { get; set; }
    }
}
