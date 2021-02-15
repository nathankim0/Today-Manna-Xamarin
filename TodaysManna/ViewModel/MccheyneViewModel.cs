using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodaysManna.Models;
using TodaysManna.Views;
using static TodaysManna.Models.MccheyneData;
using static TodaysManna.Models.MccheyneRangeData;

namespace TodaysManna.ViewModel
{
    public class MccheyneViewModel : INotifyPropertyChanged
    {
        public string Today;

        private ObservableCollection<MccheyneContent> _mccheyneContents1 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents1
        {
            get
            {
                return _mccheyneContents1;
            }
        }

        private ObservableCollection<MccheyneContent> _mccheyneContents2 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents2
        {
            get
            {
                return _mccheyneContents2;
            }
        }

        private ObservableCollection<MccheyneContent> _mccheyneContents3 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents3
        {
            get
            {
                return _mccheyneContents3;
            }
        }

        private ObservableCollection<MccheyneContent> _mccheyneContents4 = new ObservableCollection<MccheyneContent>();
        public ObservableCollection<MccheyneContent> MccheyneContents4
        {
            get
            {
                return _mccheyneContents4;
            }
        }

        private MccheyneData _mccheyneData = new MccheyneData();
        public MccheyneData JsonMccheyneData
        {
            get => _mccheyneData;
            set
            {
                if (_mccheyneData != value)
                {
                    _mccheyneData = value;
                    OnPropertyChanged(nameof(JsonMccheyneData));
                }
            }
        }

        private string _verseRange;
        public string VerseRange
        {
            get => _verseRange;
            set
            {
                if (_verseRange != value)
                {
                    _verseRange = value;
                    OnPropertyChanged(nameof(VerseRange));
                }
            }
        }

        private string _displayDateRange;
        public string DisplayDateRange
        {
            get => _displayDateRange;
            set
            {
                if (_displayDateRange != value)
                {
                    _displayDateRange = value;
                    OnPropertyChanged(nameof(DisplayDateRange));
                }
            }
        }

        public MccheyneViewModel()
        {
            DateTime thisDate = GetCorrectDateLeapYear(DateTime.Now);

            GetMccheyneRange(thisDate);

            try
            {
                GetMccheyne(thisDate);
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

        public void GetMccheyne(DateTime dateTime)
        {
            MccheyneContents1.Clear();
            MccheyneContents2.Clear();
            MccheyneContents3.Clear();
            MccheyneContents4.Clear();

            //  var _verseRange = GetMccheyneRange(dateTime);
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
                            
                           // System.Diagnostics.Debug.WriteLine($"{_firstNum}장{_secondNum}절");

                            if (a != _halfVerse)
                            {
                                MccheyneContents1.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    //FullRange= _verseRange,
                                    Book = node2.Book,
                                    FirstNumber=_firstNum,
                                    SecondNumber=_secondNum,
                                    Verse = node2.Verse,
                                    FullVerse=_fullVerse + "\n\n",
                                    HalfVerse= _halfVerse,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                MccheyneContents1.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    //FullRange = _verseRange,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = "",
                                    HalfVerse = "",
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
                                    //FullRange = _verseRange,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = _fullVerse + "\n\n",
                                    HalfVerse = _halfVerse,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                MccheyneContents2.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    //FullRange = _verseRange,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = "",
                                    HalfVerse = "",
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
                                    //FullRange = _verseRange,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = _fullVerse + "\n\n",
                                    HalfVerse = _halfVerse,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                MccheyneContents3.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    //FullRange = _verseRange,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse = "",
                                    HalfVerse = "",
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
                                    //FullRange = _verseRange,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse=_fullVerse + "\n\n",
                                    HalfVerse = _halfVerse,
                                    Content = node2.Content
                                });
                            }
                            else
                            {
                                MccheyneContents4.Add(new MccheyneContent
                                {
                                    Id = node2.Id,
                                    FullRange = _verseRange,
                                    Book = node2.Book,
                                    FirstNumber = _firstNum,
                                    SecondNumber = _secondNum,
                                    Verse = node2.Verse,
                                    FullVerse="",
                                    HalfVerse = "",
                                    Content = node2.Content
                                });
                            }
                            d = _halfVerse;

                            break;
                    }

                    //System.Diagnostics.Debug.WriteLine(node2.Id);
                    //System.Diagnostics.Debug.WriteLine(node2.Book);
                    //System.Diagnostics.Debug.WriteLine(node2.Verse);
                    //System.Diagnostics.Debug.WriteLine(node2.Content);
                }
            }
        }


        private IEnumerable<Days> GetJsonBible()
        {
            var jsonFileName = "mcc.json";
            var ObjContactList = new MccheyneList();

            var assembly = typeof(MccheynePage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Datas.{jsonFileName}");

            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();  
                ObjContactList = JsonConvert.DeserializeObject<MccheyneList>(jsonString);
            }

            return ObjContactList.DaysOfMccheyne;
        }

        public void GetMccheyneRange(DateTime thisDate)
        {
            var date = thisDate.ToString("M-d");
            VerseRange = MannaViewModel.mccheyneRanges.Find(x => x.Date.Equals(date)).Range;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
