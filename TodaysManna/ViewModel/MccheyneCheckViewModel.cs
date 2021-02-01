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
using static TodaysManna.Models.MccheyneCheckData;
using static TodaysManna.Models.MccheyneData;
using static TodaysManna.Models.MccheyneRangeData;
namespace TodaysManna.ViewModel
{
    public class MccheyneCheckViewModel : INotifyPropertyChanged
    {
        List<MccheyneCheckRange> mccheyneCheckRangeList;

        public ObservableCollection<MccheyneCheckContent> _mccheyneCheckList = new ObservableCollection<MccheyneCheckContent>();
        public ObservableCollection<MccheyneCheckContent> MccheyneCheckList
        {
            get
            {
                return _mccheyneCheckList;
            }
        }

        //public class MccheyneCheckContentGroup : List<MccheyneCheckContent>
        //{
        //    public string Date { get; private set; }

        //    public MccheyneCheckContentGroup(string date, List<MccheyneCheckContent> mccheyneCheckContents) : base(mccheyneCheckContents)
        //    {
        //        Date = date;
        //    }
        //}
        //public List<MccheyneCheckContentGroup> MccheyneCheckList { get; private set; } = new List<MccheyneCheckContentGroup>();

        public MccheyneCheckViewModel()
        {
            //mccheyneRangeLists = new List<MccheyneRange>(MannaViewModel.mccheyneRanges);
            //string range = "";
            //char[] delimiterChars = {','};
            //foreach (var rangeList in mccheyneRangeLists)
            //{
            //    range = rangeList.Range;
            //    string[] words = range.Split(delimiterChars);
            //    var verseList = new List<MccheyneCheckVerse>();
            //    foreach(var word in words)
            //    {
            //        verseList.Add(new MccheyneCheckVerse
            //        {
            //            IsChecked = false,
            //            Location = word
            //        });
            //    }
            //    MccheyneCheckList.Add(new MccheyneCheckContent
            //    {
            //        Date = rangeList.Date,
            //        Verses = verseList
            //    });
            //}
            mccheyneCheckRangeList = new List<MccheyneCheckRange>();
            try
            {
                mccheyneCheckRangeList = GetJsonMccheyneRange();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("GetJsonMccheyneRange() Error");
            }
            foreach(var range in mccheyneCheckRangeList)
            {
                MccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range1
                });
                MccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range2
                });
                MccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range3
                });
                MccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range4
                });
                MccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    IsChecked = false,
                    Range = range.Range5
                });
                //MccheyneCheckList.Add(new MccheyneCheckContentGroup(range.Date, new List<MccheyneCheckContent>
                //{
                //    new MccheyneCheckContent
                //    {
                //        Date=range.Date,
                //        IsChecked=false,
                //        Range=range.Range1
                //    },
                //    new MccheyneCheckContent
                //    {
                //        Date=range.Date,
                //        IsChecked=false,
                //        Range=range.Range2
                //    },
                //    new MccheyneCheckContent
                //    {
                //        Date=range.Date,
                //        IsChecked=false,
                //        Range=range.Range3
                //    },
                //    new MccheyneCheckContent
                //    {
                //        Date=range.Date,
                //        IsChecked=false,
                //        Range=range.Range4
                //    },
                //    new MccheyneCheckContent
                //    {
                //        Date=range.Date,
                //        IsChecked=false,
                //        Range=range.Range5
                //    },
                //}));
            }
            
        }
        private List<MccheyneCheckRange> GetJsonMccheyneRange()
        {
            string jsonFileName = "MccheyneRange2.json";
            MccheyneCheckRangeList ObjContactList = new MccheyneCheckRangeList();


            var assembly = typeof(SettingPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();

                //Converting JSON Array Objects into generic list    
                ObjContactList = JsonConvert.DeserializeObject<MccheyneCheckRangeList>(jsonString);
            }

            return ObjContactList.CheckRanges;
        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
