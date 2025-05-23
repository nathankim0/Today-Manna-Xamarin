﻿using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using System.Threading.Tasks;
using TodaysManna.Models;
using TodaysManna.Views;
using Plugin.StoreReview;

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

            RequestReview();
        }

        private async void RequestReview()
        {
            if (Preferences.Get("numberOfOpenApp", 0) == 2)
            {
                await CrossStoreReview.Current.RequestReview(false);
            }
            Preferences.Set("numberOfOpenApp", Preferences.Get("numberOfOpenApp", 0) + 1);
        }

        private async void ShowNoticePopup()
        {
            if(Preferences.Get("isNoticeDisabled", false))
            {
                return;
            }
            Preferences.Set("isNoticeDisabled", true);

            await DisplayAlert("공지", "📌 현재 '만나' 기능이 여러 버그와 서버 비용 부담으로 인해 삭제 되었습니다. 추후에 다시 추가될 예정이오니 잠시 기다려주세요!\n📌 체크리스트에 안읽은 맥체인 모아보기 기능이 추가 되었습니다.\n📌 설정에 후원 기능이 추가 되었습니다.", "확인");
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
            var shareText = $"{AppManager.GetShareTopTextString()}\n맥체인: {viewModel.MccheyneRange}\n{AppManager.GetShareBottomTextString()}";

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