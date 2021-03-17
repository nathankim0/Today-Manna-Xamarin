using System;
using System.Threading.Tasks;

namespace TodaysManna.Models
{
    public static class WidgetService
    {
        public static string WidgetString = "";
        public static EventHandler<string> widgetReady;

        public static async void SetRangeOnWidgetAsync()
        {
            var _restService = new RestService();

            var JsonMannaData = new MannaData();
            JsonMannaData =  await _restService.GetMannaDataAsync(Constants.MannaEndpoint);

            var today = DateTime.Now.ToString("M-d");
            var todayMccheyneRange = App.mccheyneRanges.Find(x => x.Date.Equals(today)).Range;

            var MannaShareRange = $"만나: {JsonMannaData.Verse}";
            var McShareRange = $"맥체인: {todayMccheyneRange}";

            WidgetString = DateTime.Now.ToString("yyyy-MM-dd (ddd)") + "\n" + MannaShareRange + "\n" + McShareRange;

            widgetReady.Invoke(null, WidgetString);
        }
    }
}
