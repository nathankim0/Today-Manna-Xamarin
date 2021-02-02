using System;
using System.Collections.Generic;

namespace TodaysManna.Models
{
    public class MccheyneCheckContent
    {
        public string Date { get; set; }
        public string[] Ranges { get; set; } = new string[5];
        public bool[] IsChecked { get; set; } = new bool[5];
        public bool Range5IsNull { get; set; }
    }
}
