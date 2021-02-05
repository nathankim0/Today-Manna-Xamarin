using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Newtonsoft.Json;
using TodaysManna.Models;
using TodaysManna.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static TodaysManna.Models.MccheyneCheckData;

namespace TodaysManna.ViewModel
{
    public class MccheyneCheckViewModel : INotifyPropertyChanged
    {
        private readonly List<MccheyneCheckRange> mccheyneCheckRangeList;

        private ObservableCollection<MccheyneCheckContent> _mccheyneCheckList = new ObservableCollection<MccheyneCheckContent>();
        public ObservableCollection<MccheyneCheckContent> MccheyneCheckList
        {
            get => _mccheyneCheckList;
            set
            {
                if (_mccheyneCheckList != value)
                {
                    _mccheyneCheckList = value;
                    OnPropertyChanged(nameof(MccheyneCheckList));
                }
            }
            
        }
            public ICommand command { get; set; }

        public MccheyneCheckViewModel()
        {
            command = new Command<string>(SetCheck);

            mccheyneCheckRangeList = new List<MccheyneCheckRange>();
            try
            {
                mccheyneCheckRangeList = GetJsonMccheyneRange();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail("GetJsonMccheyneRange() Error");
                System.Diagnostics.Debug.Fail(e.Message);
            }

            SetCheckRangeList();

            try
            {
                SetCheckedItem();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail(e.Message);
            }
        }

        private void SetCheckRangeList()
        {
            int i = 0;

            foreach (var range in mccheyneCheckRangeList)
            {
                //var dic = new Dictionary<int, string>();

                //dic.Add(1, range.Range1);
                //dic.Add(2, range.Range2);
                //dic.Add(3, range.Range3);
                //dic.Add(4, range.Range4);
                //dic.Add(5, range.Range5);

                var range5IsNull = true;
                if (range.Range5 == "")
                {
                    range5IsNull = false;
                }

                MccheyneCheckList.Add(new MccheyneCheckContent
                {
                    Date = range.Date,
                    Ranges = new MccheyneOneRange[]
                    {
                        new MccheyneOneRange
                        {
                            Id=i.ToString(),
                            IsChecked=false,
                            Color=Color.White,
                            RangeText=range.Range1
                        },
                        new MccheyneOneRange
                        {
                            Id=(++i).ToString(),
                            IsChecked=false,
                            Color=Color.White,
                            RangeText=range.Range2
                        },
                        new MccheyneOneRange
                        {
                            Id=(++i).ToString(),
                            IsChecked=false,
                            Color=Color.White,
                            RangeText=range.Range3
                        },
                        new MccheyneOneRange
                        {
                            Id=(++i).ToString(),
                            IsChecked=false,
                            Color=Color.White,
                            RangeText=range.Range4
                        },
                        new MccheyneOneRange
                        {
                            Id=(++i).ToString(),
                            IsChecked=false,
                            Color=Color.White,
                            RangeText=range.Range5
                        }
                    },
                    Range5IsNull = range5IsNull
                });
                ++i;
            }
        }

        private void SetCheckedItem()
        {
            MccheyneCheckList.ForEach(x =>
            {
                x.Ranges[0].IsChecked = Preferences.Get(x.Ranges[0]?.Id, false);
                x.Ranges[0].Color = x.Ranges[0].IsChecked == true ? Color.SkyBlue : Color.White;

                //Console.WriteLine("RangeText: " + x.Ranges[0].RangeText);
                //Console.WriteLine("IsChecked: " + x.Ranges[0].IsChecked);

                x.Ranges[1].IsChecked = Preferences.Get(x.Ranges[1]?.Id, false);
                x.Ranges[1].Color = x.Ranges[1].IsChecked == true ? Color.LightPink : Color.White;

                //Console.WriteLine("RangeText: " + x.Ranges[1].RangeText);
                //Console.WriteLine("IsChecked: " + x.Ranges[1].IsChecked);

                x.Ranges[2].IsChecked = Preferences.Get(x.Ranges[2]?.Id, false);
                x.Ranges[2].Color = x.Ranges[2].IsChecked == true ? Color.LightGreen : Color.White;

                //Console.WriteLine("RangeText: " + x.Ranges[2].RangeText);
                //Console.WriteLine("IsChecked: " + x.Ranges[2].IsChecked);

                x.Ranges[3].IsChecked = Preferences.Get(x.Ranges[3]?.Id, false);
                x.Ranges[3].Color = x.Ranges[3].IsChecked == true ? Color.Yellow : Color.White;

                //Console.WriteLine("RangeText: " + x.Ranges[3].RangeText);
                //Console.WriteLine("IsChecked: " + x.Ranges[3].IsChecked);

                x.Ranges[4].IsChecked = Preferences.Get(x.Ranges[4]?.Id, false);

                x.Ranges[4].Color = x.Ranges[4].IsChecked == true ? Color.MediumPurple : Color.White;

                //Console.WriteLine("RangeText: " + x.Ranges[4].RangeText);
                //Console.WriteLine("IsChecked: " + x.Ranges[4].IsChecked);
            });
        }

        void SetCheck(string val)
        {
            MccheyneCheckList.ForEach(x =>
            {
                if (x.Ranges[0].Id == val)
                {
                    x.Ranges[0].IsChecked = !x.Ranges[0].IsChecked;
                    x.Ranges[0].Color = x.Ranges[0].IsChecked == true ? Color.SkyBlue : Color.White;
                    Preferences.Set(val, x.Ranges[0].IsChecked);
                }
                else if (x.Ranges[1].Id == val)
                {
                    x.Ranges[1].IsChecked = !x.Ranges[1].IsChecked;
                    x.Ranges[1].Color = x.Ranges[1].IsChecked == true ? Color.LightPink : Color.White;
                    Preferences.Set(val, x.Ranges[1].IsChecked);
                }
                else if (x.Ranges[2].Id == val)
                {
                    x.Ranges[2].IsChecked = !x.Ranges[2].IsChecked;
                    x.Ranges[2].Color = x.Ranges[2].IsChecked == true ? Color.LightGreen : Color.White;
                    Preferences.Set(val, x.Ranges[2].IsChecked);
                }
                else if (x.Ranges[3].Id == val)
                {
                    x.Ranges[3].IsChecked = !x.Ranges[3].IsChecked;
                    x.Ranges[3].Color = x.Ranges[3].IsChecked == true ? Color.Yellow : Color.White;
                    Preferences.Set(val, x.Ranges[3].IsChecked);
                }
                else if (x.Ranges[4].Id == val)
                {
                    x.Ranges[4].IsChecked = !x.Ranges[4].IsChecked;
                    x.Ranges[4].Color = x.Ranges[4].IsChecked == true ? Color.MediumPurple : Color.White;
                    Preferences.Set(val, x.Ranges[4].IsChecked);
                }
            });
        }


        private List<MccheyneCheckRange> GetJsonMccheyneRange()
        {
            string jsonFileName = "MccheyneRange2.json";
            MccheyneCheckRangeList ObjContactList = new MccheyneCheckRangeList();


            var assembly = typeof(MccheyneCheckListPage).GetTypeInfo().Assembly;
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
