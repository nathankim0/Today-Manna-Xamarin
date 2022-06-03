using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using System.Threading.Tasks;
using TodaysManna.Models;
using TodaysManna.Views;

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        public MannaPage()
        {
            InitializeComponent();
            Padding = new Thickness(0, Constants.StatusBarHeight, 0, 0);

            var viewModel = new MannaViewModel();
            BindingContext = viewModel;

            viewModel.IsRefreshing = true;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMannaToTop, (sender) =>
            {
                outerScrollView.ScrollToAsync(0, 0, true);
            });

            ShowNoticePopup();
        }

        private async void ShowNoticePopup()
        {
            if(Preferences.Get(DateTime.Today.ToString(), false))
            {
                return;
            }
            Preferences.Set(DateTime.Today.ToString(), true);

            await DisplayAlert("공지", "만나 기능이 여러 버그와 서버 비용 부담으로 인해 삭제 되었습니다. 새로운 앱으로 개발 진행 중이오니 양해 부탁드립니다. 감사합니다 ㅠㅡㅠ", "확인");
        }

        bool isFirstView = true;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!(BindingContext is MannaViewModel viewModel)) return;
            viewModel.CustomFontSize = AppManager.GetCurrentTextSize();

            if (!isFirstView)
            {
                viewModel.SetTodayCheckList();
            }
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.IsLoadingServer = true;
            viewModel.IsRefreshing = false;

            viewModel.SetTodayCheckList();
            isFirstView = false;
            viewModel.IsLoadingServer = false;
        }


        private async void OnSettingClicked(object sender, EventArgs e)
        {
            var settingPage = new SettingPage();
            await Navigation.PushAsync(settingPage);
        }


        private async void OnShareSettingClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShareSettingPage());
        }

        private async void OnShareMannaAndMccheyneRangeButtonTapped(object sender, EventArgs e)
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;
            var shareText = $"맥체인: {viewModel.MccheyneRange}\n{AppManager.GetShareBottomTextString()}";

            await SaveToClipboard(shareText);
        }

        private async Task SaveToClipboard(string shareText)
        {
            await Clipboard.SetTextAsync(shareText);

            string title;
            string ok;
            if (AppManager.GetCurrentLanguageString() == Language.Korean.ToString())
            {
                title = "클립보드에 복사됨";
                ok = "확인";
            }
            else
            {
                title = "Copied to clipboard";
                ok = "Ok";
            }

            await DisplayAlert(title, shareText, ok);
        }

        void OnMccheyneCheckTapped(object sender, EventArgs e)
        {
            var mccheyneOneRange = ((TappedEventArgs)e).Parameter as MccheyneOneRange;
            mccheyneOneRange.IsChecked = !mccheyneOneRange.IsChecked;
        }
    }
}