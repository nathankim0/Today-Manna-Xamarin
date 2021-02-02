using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TodaysManna.Models;
using TodaysManna.Views;
using static TodaysManna.Models.MccheyneCheckData;

namespace TodaysManna.ViewModel
{
    public class MccheyneCheckViewModel : INotifyPropertyChanged
    {
        private readonly List<MccheyneCheckRange> mccheyneCheckRangeList;

        private ObservableCollection<MccheyneCheckContent> _mccheyneCheckList = new ObservableCollection<MccheyneCheckContent>();
        public ObservableCollection<MccheyneCheckContent> MccheyneCheckList
        {
            get
            {
                return _mccheyneCheckList;
            }
        }

        public MccheyneCheckViewModel()
        {
          
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
                //var dic = new Dictionary<int, string>();

                //dic.Add(1, range.Range1);
                //dic.Add(2, range.Range2);
                //dic.Add(3, range.Range3);
                //dic.Add(4, range.Range4);
                //dic.Add(5, range.Range5);

                var range5IsNull = true;
                if(range.Range5 == "")
                {
                    range5IsNull = false;
                }

                MccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    Ranges = new string[]
                    {
                        range.Range1, range.Range2, range.Range3, range.Range4, range.Range5
                    },
                    IsChecked = new bool[]
                    {
                        false,false,false,false,false
                    },
                    Range5IsNull= range5IsNull
                });

                //MccheyneCheckList.Add(new MccheyneCheckContent
                //{
                //    Date = range.Date,
                //    IsChecked = false,
                //    Range = range.Range2
                //});
                //MccheyneCheckList.Add(new MccheyneCheckContent
                //{
                //    Date = range.Date,
                //    IsChecked = false,
                //    Range = range.Range3
                //});
                //MccheyneCheckList.Add(new MccheyneCheckContent
                //{
                //    Date = range.Date,
                //    IsChecked = false,
                //    Range = range.Range4
                //});
                //MccheyneCheckList.Add(new MccheyneCheckContent
                //{
                //    Date = range.Date,
                //    IsChecked = false,
                //    Range = range.Range5
                //});
              
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
