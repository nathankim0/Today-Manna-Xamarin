using System;
using System.Threading.Tasks;

namespace TodaysManna.Models
{
    public class WidgetService
    {
        public WidgetService()
        {
        }

        public async Task<string> GetRangeOnWidgetAsync()
        {
            var _restService = new RestService();

            var JsonMannaData = new MannaData();
            JsonMannaData =  await _restService.GetMannaDataAsync(Constants.MannaEndpoint);

            var today = DateTime.Now.ToString("M-d");
            var todayMccheyneRange = App.mccheyneRanges.Find(x => x.Date.Equals(today)).Range;

            var MannaShareRange = $"만나: {JsonMannaData.Verse}";
            var McShareRange = $"맥체인: {todayMccheyneRange}";

            return DateTime.Now.ToString("yyyy-MM-dd (ddd)") + "\n" + MannaShareRange + "\n" + McShareRange;
        }
    }
}
