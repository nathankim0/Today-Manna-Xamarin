using System;
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
    }
}
