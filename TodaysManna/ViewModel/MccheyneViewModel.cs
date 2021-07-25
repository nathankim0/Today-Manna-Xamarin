using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodaysManna.Models;
using static TodaysManna.Models.JsonMccheyneContentModel;

namespace TodaysManna.ViewModel
{
    public class MccheyneViewModel : PageBaseViewModel
    {
        public string Today;

        private ObservableCollection<MccheyneContent> _mccheyneContents1 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents1 { get => _mccheyneContents1; set => SetProperty(ref _mccheyneContents1, value); }

        private ObservableCollection<MccheyneContent> _mccheyneContents2 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents2 { get => _mccheyneContents2; set => SetProperty(ref _mccheyneContents2, value); }

        private ObservableCollection<MccheyneContent> _mccheyneContents3 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents3 { get => _mccheyneContents3; set => SetProperty(ref _mccheyneContents3, value); }

        private ObservableCollection<MccheyneContent> _mccheyneContents4 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents4 { get => _mccheyneContents4; set => SetProperty(ref _mccheyneContents4, value); }

        private JsonMccheyneContentModel _JsonMccheyneData = new JsonMccheyneContentModel();
        public JsonMccheyneContentModel JsonMccheyneData { get => _JsonMccheyneData; set => SetProperty(ref _JsonMccheyneData, value); }

        private string _mccheyneRange;
        public string MccheyneRange { get => _mccheyneRange; set => SetProperty(ref _mccheyneRange, value); }

        private string _displayDateRange;
        public string DisplayDateRange { get => _displayDateRange; set => SetProperty(ref _displayDateRange, value); }

        public MccheyneViewModel()
        {
            var thisDate = GetCorrectDateLeapYear(DateTime.Now);

            _ = GetMccheyneRange(thisDate);

            try
            {
                _ = GetMccheyne(thisDate);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("GetMccheyne() Error");
            }
        }

        public static DateTime GetCorrectDateLeapYear(DateTime newDate)
        {
            var dateNow = newDate;
            var thisDate = dateNow;
            if (DateTime.IsLeapYear(dateNow.Year) && ((dateNow.Month == 2 && dateNow.Day > 28) || (dateNow.Month > 2)))
            {
                thisDate = thisDate.AddDays(1);
                if (dateNow.Month == 12 && dateNow.Day == 31)
                {
                    thisDate = dateNow;
                }
            }

            return thisDate;
        }

        public Task GetMccheyne(DateTime dateTime)
        {
            MccheyneContents1.Clear();
            MccheyneContents2.Clear();
            MccheyneContents3.Clear();
            MccheyneContents4.Clear();

            DisplayDateRange = dateTime.ToString("yyyy년 MM월 dd일 (ddd)");
            var dateTimeString=dateTime.ToString("M_d");

            var daysOfMccheynes = GetJsonBible();

            var todayProperty = "Mccheynes" + dateTimeString;

            foreach (var node in daysOfMccheynes)
            {
                var t= node.GetType().GetProperty(todayProperty).GetValue(node, null) as List<Mccheyne>;

                var a = "";
                var b = "";
                var c = "";
                var d = "";

                foreach (var node2 in t)
                {
                    var _firstNum = node2.Verse.Substring(0, node2.Verse.IndexOf(":"));
                    var _secondNum = node2.Verse.Substring(node2.Verse.IndexOf(":")+1);
                    var _fullVerse = $"{node2.Book} {node2.Verse}";
                    var _halfVerse = $"{node2.Book} {_firstNum}";
                    switch (node2.Id)
                    {
                        case "1":
                            if (a != _halfVerse)
                            {
                                MccheyneContents1.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber=_firstNum,
                                    SecondNumber=_secondNum,
                                    Verse = node2.Verse,
                                    FullVerse=_fullVerse + "\n\n",
                                    HalfVerse= _halfVerse,
                                    IsHalfVerseVisible=true,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                MccheyneContents1.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = "",
                                    HalfVerse = "",
                                    IsHalfVerseVisible=false,
                                    Content = node2.Content
                                });
                            }
                            a = _halfVerse;
                           
                            break;
                        case "2":
                            if (b != _halfVerse)
                            {
                                MccheyneContents2.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = _fullVerse + "\n\n",
                                    HalfVerse = _halfVerse,
                                    IsHalfVerseVisible = true,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                MccheyneContents2.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = "",
                                    HalfVerse = "",
                                    IsHalfVerseVisible = false,
                                    Content = node2.Content
                                });
                            }
                            b = _halfVerse;

                            break;
                        case "3":
                            if (c != _halfVerse)
                            {
                                MccheyneContents3.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = _fullVerse + "\n\n",
                                    HalfVerse = _halfVerse,
                                    IsHalfVerseVisible = true,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                MccheyneContents3.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = "",
                                    HalfVerse = "",
                                    IsHalfVerseVisible = false,
                                    Content = node2.Content
                                });
                            }
                            c = _halfVerse;

                            break;
                        case "4":
                            if (d != _halfVerse)
                            {
                                MccheyneContents4.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse=_fullVerse + "\n\n",
                                    HalfVerse = _halfVerse,
                                    IsHalfVerseVisible = true,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                MccheyneContents4.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    FullRange = _mccheyneRange,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse="",
                                    HalfVerse = "",
                                    IsHalfVerseVisible = false,
                                    Content = node2.Content
                                });
                            }
                            d = _halfVerse;

                            break;
                    }
                }
            }

            return Task.CompletedTask;
        }

        private IEnumerable<Days> GetJsonBible()
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

        public Task GetMccheyneRange(DateTime thisDate)
        {
            var findMccheyneDate = thisDate.ToString("M-d");
            //MccheyneRange = App.mccheyneRanges.Find(x => x.Date.Equals(findMccheyneDate)).Range;
            var rangeOfDate = App.mccheyneRanges.Find(x => x.Date.Equals(findMccheyneDate));
            MccheyneRange = $"{rangeOfDate.Range1} {rangeOfDate.Range2} {rangeOfDate.Range3} {rangeOfDate.Range4} {rangeOfDate.Range5}";
            return Task.CompletedTask;
        }
    }
}
