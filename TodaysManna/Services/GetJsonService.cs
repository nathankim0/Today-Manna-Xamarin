using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using TodaysManna.Models;
using TodaysManna.Models.JsonMccheyneContentModel;

namespace TodaysManna.Services
{
    public static class GetJsonService
    {
        public static List<MccheyneRange> GetMccheyneRangesFromJson()
        {
            const string jsonFileName = "MccheyneRange.json";
            var ObjContactList = new MccheyneRanges();

            var assembly = typeof(MccheyneCheckListPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.JsonFiles.{jsonFileName}");
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                ObjContactList = JsonConvert.DeserializeObject<MccheyneRanges>(jsonString);
            }

            return ObjContactList.Ranges;
        }

        public static IEnumerable<Days> GetMccheyneBibleTextsFromJson()
        {
            var jsonFileName = "mcc.json";
            var ObjContactList = new MccheyneList();

            var assembly = typeof(MccheynePage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.JsonFiles.{jsonFileName}");

            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                ObjContactList = JsonConvert.DeserializeObject<MccheyneList>(jsonString);
            }

            return ObjContactList.DaysOfMccheyne;
        }
    }
}
