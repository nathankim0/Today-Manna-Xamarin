using SQLite;
using System;

namespace TodaysManna
{
    public class MemoItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Verse { get; set; }
        public string Note { get; set; }
    }
}
