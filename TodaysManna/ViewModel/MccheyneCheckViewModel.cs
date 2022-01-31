using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TodaysManna.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

using System.Diagnostics;
using TodaysManna.Managers;

namespace TodaysManna.ViewModel
{
    public class MccheyneCheckViewModel : PageBaseViewModel
    {
        public ObservableCollection<MccheyneCheckListContent> MccheyneCheckList
        {
            get => MccheyneCheckListManager.MccheyneCheckList;
            set => SetProperty(ref MccheyneCheckListManager.MccheyneCheckList, value);
        }

        public ICommand command => new Command<string>(OnCheckButtonTapped);
        public ICommand easterEggCommand => new Command<string>(OnDateTapped);
        public ICommand goToReadCommand => new Command<string>(OnGoToReadButtonTapped);

        public MccheyneCheckViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public void OnCheckListPropertyChanged()
        {
            OnPropertyChanged(nameof(MccheyneCheckList));
        }

        private void OnCheckButtonTapped(string val)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            MccheyneCheckList.ForEach(x =>
            {
                if (x.Ranges[0].Id == val)
                {
                    x.Ranges[0].IsChecked = !x.Ranges[0].IsChecked;
                    //x.Ranges[0].Color = x.Ranges[0].IsChecked == true ? Colors.MccheyneColor1 : Color.White;
                }
                else if (x.Ranges[1].Id == val)
                {
                    x.Ranges[1].IsChecked = !x.Ranges[1].IsChecked;
                    //x.Ranges[1].Color = x.Ranges[1].IsChecked == true ? Colors.MccheyneColor2 : Color.White;
                }
                else if (x.Ranges[2].Id == val)
                {
                    x.Ranges[2].IsChecked = !x.Ranges[2].IsChecked;
                    //x.Ranges[2].Color = x.Ranges[2].IsChecked == true ? Colors.MccheyneColor3 : Color.White;
                }
                else if (x.Ranges[3].Id == val)
                {
                    x.Ranges[3].IsChecked = !x.Ranges[3].IsChecked;
                    //x.Ranges[3].Color = x.Ranges[3].IsChecked == true ? Colors.MccheyneColor4 : Color.White;
                }
                else if (x.Ranges[4].Id == val)
                {
                    x.Ranges[4].IsChecked = !x.Ranges[4].IsChecked;
                    //x.Ranges[4].Color = x.Ranges[4].IsChecked == true ? Colors.MccheyneColor5 : Color.White;
                }
            });
        }

        private void OnDateTapped(string date)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("checklist_easteregg");
            Debug.WriteLine("**** EasterEgg Invoked! ****");
          
            if (date.Equals("8-12"))
            {
                DependencyService.Get<IHapticFeedback>().Run();
            }
        }

        private void OnGoToReadButtonTapped(string date)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("checklist_go_to_read");
            Debug.WriteLine("**** Go To Read Tapped! ****");
            DependencyService.Get<IHapticFeedback>().Run();

            try
            {
                var masterPage = Application.Current.MainPage as TabbedPage;
                masterPage.CurrentPage = masterPage.Children[1];

                var toConvertDateTime = $"{DateTime.Today.Year}-{date}";
                MessagingCenter.Send(this, "goToReadTapped", Convert.ToDateTime(toConvertDateTime));
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
            }
        }
    }
}