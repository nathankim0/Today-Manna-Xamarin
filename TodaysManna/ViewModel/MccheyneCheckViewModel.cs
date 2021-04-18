﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Newtonsoft.Json;
using TodaysManna.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static TodaysManna.Models.JsonMccheyneCheckListModel;

namespace TodaysManna.ViewModel
{
    public class MccheyneCheckViewModel : BaseViewModel
    {
        private readonly List<MccheyneCheckRange> mccheyneCheckRangeList;

        private ObservableCollection<MccheyneCheckListContent> _mccheyneCheckList = new ObservableCollection<MccheyneCheckListContent>();
        public ObservableCollection<MccheyneCheckListContent> MccheyneCheckList { get => _mccheyneCheckList; set => SetProperty(ref _mccheyneCheckList, value); }

        public ICommand command { get; set; }
        public ICommand easterEggCommand { get; set; }

        public INavigation Navigation { get; set; }

        public MccheyneCheckViewModel(INavigation navigation)
        {
            Navigation = navigation;

            command = new Command<string>(OnCheckButtonTabbed);
            easterEggCommand = new Command<string>(OnDateTabbed);

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
            var i = 0;

            foreach (var range in mccheyneCheckRangeList)
            {
                var dateColor = range.Date == DateTime.Now.ToString("M-d") ? Color.Accent : Color.Default;
                var range5IsNull = true;
                if (range.Range5 == "")
                {
                    range5IsNull = false;
                }

                MccheyneCheckList.Add(new MccheyneCheckListContent
                {
                    Date = range.Date,
                    Ranges = new MccheyneOneRange[]
                    {
                        new MccheyneOneRange
                        {
                            Id=i.ToString(),
                            DateColor=dateColor,
                            IsChecked=false,
                            Color=Color.White,
                            RangeText=range.Range1
                        },
                        new MccheyneOneRange
                        {
                            Id=(++i).ToString(),
                            DateColor=dateColor,
                            IsChecked=false,
                            Color=Color.White,
                            RangeText=range.Range2
                        },
                        new MccheyneOneRange
                        {
                            Id=(++i).ToString(),
                            DateColor=dateColor,
                            IsChecked=false,
                            Color=Color.White,
                            RangeText=range.Range3
                        },
                        new MccheyneOneRange
                        {
                            Id=(++i).ToString(),
                            DateColor=dateColor,
                            IsChecked=false,
                            Color=Color.White,
                            RangeText=range.Range4
                        },
                        new MccheyneOneRange
                        {
                            Id=(++i).ToString(),
                            DateColor=dateColor,
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
                for(int i = 0; i < 5; i++)
                {
                    x.Ranges[i].IsChecked = Preferences.Get(x.Ranges[i]?.Id, false);
                }
                x.Ranges[0].Color = x.Ranges[0].IsChecked == true ? Color.SkyBlue : Color.White;
                x.Ranges[1].Color = x.Ranges[1].IsChecked == true ? Color.LightPink : Color.White;
                x.Ranges[2].Color = x.Ranges[2].IsChecked == true ? Color.LightGreen : Color.White;
                x.Ranges[3].Color = x.Ranges[3].IsChecked == true ? Color.Yellow : Color.White;
                x.Ranges[4].Color = x.Ranges[4].IsChecked == true ? Color.MediumPurple : Color.White;
            });
        }

        private void OnCheckButtonTabbed(string val)
        {
            try
            {
                // Perform click feedback
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
            FirebaseEventService.SendEventOnPlatformSpecific("mccheynchecklist_check");

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
        static int tabcount = 0;
        private void OnDateTabbed(string date)
        {
            System.Diagnostics.Debug.WriteLine("**** EasterEgg Invoked! ****");
            if (date.Equals("3-27"))
            {
                tabcount++;
                if (tabcount >= 2)
                {
                    tabcount = 0;
                }
            }
            if (date.Equals("8-12"))
            {
            }
        }

        private List<MccheyneCheckRange> GetJsonMccheyneRange()
        {
            const string jsonFileName = "MccheyneRange2.json";
            var ObjContactList = new MccheyneCheckRangeList();

            var assembly = typeof(MccheyneCheckListPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Datas.{jsonFileName}");
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                ObjContactList = JsonConvert.DeserializeObject<MccheyneCheckRangeList>(jsonString);
            }

            return ObjContactList.CheckRanges;
        }
    }
}
