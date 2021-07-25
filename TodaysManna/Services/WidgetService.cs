using System;
using System.Collections.Generic;
using TodaysManna.Models;
using TodaysManna.Services;
using static TodaysManna.Constants;

namespace TodaysManna
{
    public static class WidgetService
    {
        public static string WidgetString = "";
        public static EventHandler<string> widgetReady;
        public static List<MccheyneRange> mccheyneRanges;

        public static async void SetRangeOnWidgetAsync()
        {
            var _restService = new RestService();
            var JsonMannaData = new JsonMannaModel();

            CreateData();

            try
            {
                JsonMannaData = await _restService.GetMannaDataAsync(Rests.MannaEndpoint);

                var findMccheyneDate = DateTime.Now.ToString("M-d");
                var rangeOfDate = App.mccheyneRanges.Find(x => x.Date.Equals(findMccheyneDate));
                var todayMccheyneRange = $"{rangeOfDate.Range1} {rangeOfDate.Range2} {rangeOfDate.Range3} {rangeOfDate.Range4} {rangeOfDate.Range5}";

                var MannaShareRange = $"만나: {JsonMannaData.Verse}";
                var McShareRange = $"맥체인: {todayMccheyneRange}";

                WidgetString = DateTime.Now.ToString("G") + "\n" + MannaShareRange + "\n" + McShareRange;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail("# App GetJsonMccheyneRange() \n" + e.Message);

            }
            finally
            {
                widgetReady.Invoke(null, WidgetString);
            }
        }

        private static void CreateData()
        {
            mccheyneRanges = new List<MccheyneRange>();
            try
            {
                mccheyneRanges = GetJsonService.GetMccheyneRangesFromJson();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail("# App GetJsonMccheyneRange() \n" + e.Message);
            }
        }
    }
}
