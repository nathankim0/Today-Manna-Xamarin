using System;
using Plugin.LocalNotification;
using Plugin.StoreReview;
using TodaysManna.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class SettingPage : ContentPage
    {
        SettingViewModel viewModel;
        public SettingPage()
        {
            InitializeComponent();
            viewModel = new SettingViewModel(Navigation);
            BindingContext = viewModel;

            if (Preferences.ContainsKey("AlertTime"))
            {
                var settedTime = Preferences.Get("AlertTime", "");
                timePicker.Time = TimeSpan.Parse(settedTime);
            }

            if (Preferences.ContainsKey("IsAlert"))
            {
                var isAlert = Preferences.Get("IsAlert", false);
                alertSwitch.IsToggled = isAlert;
                if (!alertSwitch.IsToggled)
                {
                    NotificationCenter.Current.Cancel(100);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as SettingViewModel).GetAuthAndIsDatabase();

            
        }

        private async void OnBackupButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_backup");

            if (!await Application.Current.MainPage.DisplayAlert("", "백업 하시겠습니까? 저장된 백업에 덮어씌웁니다.", "확인", "취소"))
            {
                IsBusy = false;
                return;
            }
            IsBusy = true;

            await viewModel.saveDropBoxService.Authorize();
        }

        private async void OnRestoreButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_restore");

            if (!await Application.Current.MainPage.DisplayAlert("", "복원 하시겠습니까? 기존 메모는 사라집니다.", "확인", "취소"))
            {
                IsBusy = false;
                return;
            }
            IsBusy = true;

            await viewModel.restoreDropBoxService.Authorize();
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_logout");

            if (!await Application.Current.MainPage.DisplayAlert("", "백업 연결된 드롭박스에서 로그아웃 하시겠습니까?", "확인", "취소"))
            {
                IsBusy = false;
                return;
            }
            IsBusy = true;

            viewModel.Logout();
        }


        private async void OnOpenStoreButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_review");

            //CrossStoreReview.Current.OpenStoreListing("1547824358");
            //CrossStoreReview.Current.OpenStoreReviewPage("1547824358");
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    await CrossStoreReview.Current.RequestReview(false);
                    break;
                case Device.Android:
                    var uri = new Uri("https://play.google.com/store/apps/details?id=com.manna.parsing2");
                    await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                    break;
                default:
                    break;
            }
        }

        private async void OnReportButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_report");

            var address = "jinyeob07@gmail.com";
            await Clipboard.SetTextAsync(address);
            await Application.Current.MainPage.DisplayAlert("클립보드에 복사됨", address, "확인");
        }

        private async void OnDonateButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_donate");

            var uri = new Uri("https://qr.kakaopay.com/281006011000037630355680");
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        private void OnTimePickerPropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
            {
                SetTriggerTime();
            }
        }

        private void SetTriggerTime()
        {
            if (!alertSwitch.IsToggled) return;

            NotificationCenter.Current.CancelAll();

            var alertTime = timePicker.Time.ToString();
            Preferences.Set("AlertTime", alertTime);

            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "오늘의 만나",
                Description = "오늘의 만나를 함께 만나요!",
                Repeats = NotificationRepeat.Daily,
                NotifyTime = DateTime.Today + timePicker.Time,
                ReturningData = "Dummy data",
            };
            NotificationCenter.Current.Show(notification);

        }

        void Switch_Toggled(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (alertSwitch.IsToggled)
            {
                SetTriggerTime();
                Preferences.Set("IsAlert", true);
            }
            else
            {
                NotificationCenter.Current.Cancel(100);
                Preferences.Set("IsAlert", false);
            }
        }
    }
}
