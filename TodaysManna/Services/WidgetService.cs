using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using TodaysManna.Models;
using static TodaysManna.Models.JsonMccheyneRangeModel;

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
                JsonMannaData = await _restService.GetMannaDataAsync(Constants.MannaEndpoint);

                var today = DateTime.Now.ToString("M-d");
                var todayMccheyneRange = mccheyneRanges.Find(x => x.Date.Equals(today)).Range;

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
                mccheyneRanges = GetJsonMccheyneRange();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail("# App GetJsonMccheyneRange() \n" + e.Message);
            }
        }

        private static List<MccheyneRange> GetJsonMccheyneRange()
        {
            var jsonFileName = "MccheyneRange.json";
            var ObjContactList = new MccheyneRangeList();
            var assembly = typeof(MannaPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Datas.{jsonFileName}");

            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                ObjContactList = JsonConvert.DeserializeObject<MccheyneRangeList>(jsonString);
            }

            return ObjContactList.Ranges;
        }
    }
}
