using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TodaysManna.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using TodaysManna.Constants;

namespace TodaysManna.ViewModel
{
    public class MccheyneCheckViewModel : PageBaseViewModel
    {
        private ObservableCollection<MccheyneCheckListContent> _mccheyneCheckList = new ObservableCollection<MccheyneCheckListContent>();
        public ObservableCollection<MccheyneCheckListContent> MccheyneCheckList { get => _mccheyneCheckList; set => SetProperty(ref _mccheyneCheckList, value); }

        public ICommand command => new Command<string>(OnCheckButtonTabbed);
        public ICommand easterEggCommand => new Command<string>(OnDateTabbed);

        public MccheyneCheckViewModel(INavigation navigation)
        {
            Navigation = navigation;

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

            foreach (var range in App.mccheyneRanges)
            {
                var dateColor = range.Date == DateTime.Now.ToString("M-d") ? Color.Accent : Color.Black;
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
                x.Ranges[0].Color = x.Ranges[0].IsChecked == true ?  Colors.MccheyneColor1 : Color.White;
                x.Ranges[1].Color = x.Ranges[1].IsChecked == true ? Colors.MccheyneColor2 : Color.White;
                x.Ranges[2].Color = x.Ranges[2].IsChecked == true ? Colors.MccheyneColor3 : Color.White;
                x.Ranges[3].Color = x.Ranges[3].IsChecked == true ? Colors.MccheyneColor4 : Color.White;
                x.Ranges[4].Color = x.Ranges[4].IsChecked == true ? Colors.MccheyneColor5 : Color.White;
            });
        }

        private void OnCheckButtonTabbed(string val)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            MccheyneCheckList.ForEach(x =>
            {
                if (x.Ranges[0].Id == val)
                {
                    x.Ranges[0].IsChecked = !x.Ranges[0].IsChecked;
                    x.Ranges[0].Color = x.Ranges[0].IsChecked == true ? Colors.MccheyneColor1 : Color.White;
                    Preferences.Set(val, x.Ranges[0].IsChecked);
                }
                else if (x.Ranges[1].Id == val)
                {
                    x.Ranges[1].IsChecked = !x.Ranges[1].IsChecked;
                    x.Ranges[1].Color = x.Ranges[1].IsChecked == true ? Colors.MccheyneColor2 : Color.White;
                    Preferences.Set(val, x.Ranges[1].IsChecked);
                }
                else if (x.Ranges[2].Id == val)
                {
                    x.Ranges[2].IsChecked = !x.Ranges[2].IsChecked;
                    x.Ranges[2].Color = x.Ranges[2].IsChecked == true ? Colors.MccheyneColor3 : Color.White;
                    Preferences.Set(val, x.Ranges[2].IsChecked);
                }
                else if (x.Ranges[3].Id == val)
                {
                    x.Ranges[3].IsChecked = !x.Ranges[3].IsChecked;
                    x.Ranges[3].Color = x.Ranges[3].IsChecked == true ? Colors.MccheyneColor4 : Color.White;
                    Preferences.Set(val, x.Ranges[3].IsChecked);
                }
                else if (x.Ranges[4].Id == val)
                {
                    x.Ranges[4].IsChecked = !x.Ranges[4].IsChecked;
                    x.Ranges[4].Color = x.Ranges[4].IsChecked == true ? Colors.MccheyneColor5 : Color.White;
                    Preferences.Set(val, x.Ranges[4].IsChecked);
                }
            });
        }

        private async void OnDateTabbed(string date)
        {
            System.Diagnostics.Debug.WriteLine("**** EasterEgg Invoked! ****");
          
            if (date.Equals("8-12"))
            {
                DependencyService.Get<IHapticFeedback>().Run();
                await Navigation.PushAsync(new QrScanPage());
            }
        }
    }
}