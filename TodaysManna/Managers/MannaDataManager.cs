using System;
using System.Collections.ObjectModel;
using TodaysManna.Models;

namespace TodaysManna.Managers
{
    public class MannaDataManager
    {
        public static ObservableCollection<MannaContent> MannaContents = new ObservableCollection<MannaContent>();
        public static JsonMannaModel JsonMannaData = new JsonMannaModel();

        public static string Today { get; set; } = DateTime.Now.ToString("yyyy년 MM월 dd일 (ddd)");
        public static string DisplayDateRange { get; set; } = DateTime.Now.ToString("MM/dd");
        public static string AllString { get; set; } = "";
        public static string MannaShareRange { get; set; } = "";
        public static string McShareRange { get; set; } = "";
        public static string TodayMccheyneRange { get; set; } = "";

        public MannaDataManager()
        {
        }
    }
}
