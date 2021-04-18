namespace TodaysManna.Models
{
    public class MccheyneContent
    {
        public string Id { get; set; } // 1~4
        public string FullRange { get; set; }
        public string Book { get; set; } // 권 (예: 창세기)
        public string FirstNumber { get; set; } // 장
        public string SecondNumber { get; set; } // 절
        public string Verse { get; set; } // 장:절
        public string FullVerse { get; set; } // 권 장:절 (예: 창세기 1:1)
        public string HalfVerse { get; set; } // 권 장 (예: 창세기 1)
        public bool IsHalfVerseVisible { get; set; }
        public string Content { get; set; } // 구절
    }
}
