using System;
using System.Collections.Generic;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using Xamarin.Essentials;
using TodaysManna.AppInterfaces;

namespace TodaysManna
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);

            _switch.IsToggled = Preferences.Get("IsAlarm", defaultValue: false);
            var t = Preferences.Get("AlarmTime", "07:00:00");
            _timePicker.Time = TimeSpan.Parse(t);
            Console.WriteLine($"**** init time : {_timePicker.Time}");
        }

        //iOS - Notification Framework (version 10 and above).
        void OnSwitchToggled(object sender, ToggledEventArgs args)
        {
            if (args.Value) // true
            {
                Preferences.Set("IsAlarm", true);
                SetDateTimeNotification();
                //DisplayAlert("Start", null, "OK");
            }
            else // false
            {
                Preferences.Set("IsAlarm", false);
                UnSetDateTimeNotification();
                //DisplayAlert("Stop", null, "OK");
            }
        }

        void SetDateTimeNotification()
        {
            string time = "";

            if (Preferences.ContainsKey("AlarmTime"))
            {
                time = Preferences.Get("AlarmTime", "07:00:00");
            }
            else
            {
                time = _timePicker.Time.ToString();
            }

            DependencyService.Get<ILocalNotification>()
               .ShowNotification("", "", "오늘도 만나와 함께 힘찬 하루를 시작하세요!", strNotificationIdItem: "1", time, isInterval: false, extraParameters: "");

            Console.WriteLine($"**** set time : {time}");

        }

        void UnSetDateTimeNotification()
        {
            DependencyService.Get<ILocalNotification>().DisableNotification();
            Console.WriteLine($"**** unset time : {_timePicker.Time}");
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

                    Console.WriteLine($"**** change time : {time}");
                    //DisplayAlert("Alert Time Changed", null, "OK");
                }
            }
        }
    }
}
