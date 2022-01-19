using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodaysManna.Models
{
    public class KoreanManna
    {
        [JsonProperty("verse")]
        public string Verse { get; set; } = "";

        [JsonProperty("contents")]
        public string[] Contents { get; set; } = { "", "" };
    }

    public class EnglishManna
    {
        [JsonProperty("verses")]
        public List<EnglishContentVerse> Verses { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }
    }

    public class EnglishContentVerse
    {
        [JsonProperty("book_id")]
        public string BookId { get; set; }

        [JsonProperty("book_name")]
        public string BookName { get; set; }

        [JsonProperty("chapter")]
        public int Chapter { get; set; }

        [JsonProperty("verse")]
        public int Verse { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    //
    public partial class SpanishManna
    {
        [JsonProperty("results")]
        public SpanishResults Results { get; set; }
    }

    public partial class SpanishResults
    {
        [JsonProperty("rv_1909")]
        public List<Content> Content { get; set; }
    }

    //
    public partial class ChineseManna
    {
        [JsonProperty("results")]
        public ChineseResults Results { get; set; }
    }

    public partial class ChineseResults
    {
        [JsonProperty("ckjv_sdt")]
        public List<Content> Content { get; set; }
    }

    //
    public partial class JapaneseManna
    {
        [JsonProperty("results")]
        public JapaneseResults Results { get; set; }
    }

    public partial class JapaneseResults
    {
        [JsonProperty("kougo")]
        public List<Content> Content { get; set; }
    }

    //
    public partial class GermanManna
    {
        [JsonProperty("results")]
        public GermanResults Results { get; set; }
    }

    public partial class GermanResults
    {
        [JsonProperty("luther")]
        public List<Content> Content { get; set; }
    }

    //
    public partial class FrenchManna
    {
        [JsonProperty("results")]
        public FrenchResults Results { get; set; }
    }

    public partial class FrenchResults
    {
        [JsonProperty("segond_1910")]
        public List<Content> Content { get; set; }
    }

    //
    public partial class HindiManna
    {
        [JsonProperty("results")]
        public HindiResults Results { get; set; }
    }

    public partial class HindiResults
    {
        [JsonProperty("irv")]
        public List<Content> Content { get; set; }
    }
    public partial class Content
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("book")]
        public int Book { get; set; }

        public string BookName { get; set; }

        [JsonProperty("chapter")]
        public int Chapter { get; set; }

        [JsonProperty("verse")]
        public int Verse { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("italics")]
        public string Italics { get; set; }

        [JsonProperty("claimed")]
        public bool Claimed { get; set; }
    }
}
