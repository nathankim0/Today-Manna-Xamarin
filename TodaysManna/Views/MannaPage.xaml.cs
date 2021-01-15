using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using TodaysManna.AppInterfaces;
using System.Linq;

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        readonly MannaViewModel mannaViewModel = new MannaViewModel();
        public MannaPage()
        {
            InitializeComponent();

            BindingContext = mannaViewModel;

            _switch.IsToggled = Preferences.Get("IsAlarm", defaultValue: false);
            var t = Preferences.Get("AlarmTime", "00:00:00");
            _timePicker.Time = TimeSpan.Parse(t);
        }

        private async void OnShareButtonClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = today.Text + "\n\n" + verse.Text + "\n\n" + mannaViewModel.AllString,
                Title = "공유"
            });
        }

        private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            var view = sender as CollectionView;
            if (view.SelectedItem == null) return;
            if (e.CurrentSelection.FirstOrDefault() == null) return;

            var manna = e.CurrentSelection.FirstOrDefault() as MannaContent;

            var verseText = verse.Text;
            var tmpRangeString = verseText.Substring(0, verseText.IndexOf(":"));

            var shareRangeString = $"({tmpRangeString}:{manna.Number}){manna.MannaString}";

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = shareRangeString,
                Title = "공유"
            });


            if (view.SelectedItem != null)
            {
                view.SelectedItem = null;
            }
        }

        //iOS - Notification Framework (version 10 and above).
        void OnSwitchToggled(object sender, ToggledEventArgs args)
        {
            if (args.Value) // true
            {
                Preferences.Set("IsAlarm", true);
                SetDateTimeNotification();
                //  SetIntervalNotification(60); // test

                DisplayAlert("Start", null, "OK");
            }
            else // false
            {
                Preferences.Set("IsAlarm", false);
                UnSetDateTimeNotification();

                DisplayAlert("Stop", null, "OK");
            }
        }

        void SetIntervalNotification(int intervalTime)
        {
            DependencyService.Get<ILocalNotification>()
               .ShowNotification("title example", "subtitle example", "description example", strNotificationIdItem: "1", intervalTime.ToString(), isInterval: true, extraParameters: "");
        }

        void SetDateTimeNotification()
        {
            string time = "";

            if (Preferences.ContainsKey("AlarmTime"))
            {
                time = Preferences.Get("AlarmTime", "00:00:00");
            }
            else
            {
                time = _timePicker.Time.ToString();
            }

            DependencyService.Get<ILocalNotification>()
               .ShowNotification("title example", "subtitle example", "description example", strNotificationIdItem: "1", time, isInterval: false, extraParameters: "");
        }

        void UnSetDateTimeNotification()
        {
            DependencyService.Get<ILocalNotification>().DisableNotification();
        }


        void OnAlarmButtonClicked(System.Object sender, System.EventArgs e)
        {
            SetIntervalNotification(60);
        }

        void OnTimePickerPropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
            {
                var time = _timePicker.Time.ToString();
                Preferences.Set("AlarmTime", time);
                if (_switch.IsToggled)
                {
                    UnSetDateTimeNotification();
                    SetDateTimeNotification();

                    DisplayAlert("Alert Time Changed", null, "OK");
                }
            }
        }
    }
}
