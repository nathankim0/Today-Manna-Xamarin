using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using System.Linq;
using System;
using NavigationPage = Xamarin.Forms.NavigationPage;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using System.Text.RegularExpressions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace TodaysManna.Views
{
    public partial class MccheyneCheckListPage : ContentPage
    {
        private OptionPopup optionPopup;
        public MccheyneCheckListPage(MccheyneCheckViewModel mccheyneCheckViewModel)
        {
            //On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.PageSheet);
            InitializeComponent();
            BindingContext = mccheyneCheckViewModel; //new MccheyneCheckViewModel();// mccheyneCheckViewModel;

            optionPopup = new OptionPopup();

            optionPopup.CheckButtonClicked += OnCheckButtonClicked;
            optionPopup.ClearButtonClicked+= OnClearButtonClicked;
        }
        private void ScrollToToday()
        {
            var todayMccheyne = (BindingContext as MccheyneCheckViewModel).MccheyneCheckList.Where(x => x.Date == DateTime.Now.ToString("M-d")).FirstOrDefault();
            collectionView.ScrollTo(todayMccheyne, null, Xamarin.Forms.ScrollToPosition.Center, false);
        }

        protected override void OnAppearing()
        {
            ScrollToToday();
        }

        private void OnScrollToToday(object sender, EventArgs e)
        {
            ScrollToToday();
        }

        private async void OnCheckButtonClicked(object sender, EventArgs e)
        {
            if (!(BindingContext is MccheyneCheckViewModel viewModel)) { return; }

            bool IsConfirmed = await DisplayAlert("오늘까지 체크", "정말 체크 하시겠습니까?", "확인", "취소");
            var today = DateTime.Now;
            if (IsConfirmed)
            {
                await PopupNavigation.Instance.PopAsync();

                viewModel.MccheyneCheckList.ForEach(x =>
                {
                    var month = int.Parse(x.Date.Substring(0, x.Date.IndexOf("-")));
                    var day = int.Parse(x.Date.Substring(x.Date.IndexOf("-") + 1));

                    if (month < today.Month || (month == today.Month && day <= today.Day))
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            x.Ranges[i].IsChecked = true;
                            Preferences.Set(x.Ranges[i].Id, x.Ranges[i].IsChecked);
                        }
                        x.Ranges[0].Color = x.Ranges[0].IsChecked == true ? Color.SkyBlue : Color.White;
                        x.Ranges[1].Color = x.Ranges[1].IsChecked == true ? Color.LightPink : Color.White;
                        x.Ranges[2].Color = x.Ranges[2].IsChecked == true ? Color.LightGreen : Color.White;
                        x.Ranges[3].Color = x.Ranges[3].IsChecked == true ? Color.Yellow : Color.White;
                        x.Ranges[4].Color = x.Ranges[4].IsChecked == true ? Color.MediumPurple : Color.White;
                    }
                });
                ScrollToToday();
            }
        }
        private async void OnClearButtonClicked(object sender, EventArgs e)
        {
            if (!(BindingContext is MccheyneCheckViewModel viewModel)) { return; }

            bool IsConfirmed = await DisplayAlert("초기화", "정말 초기화 하시겠습니까?", "확인", "취소");
            if (IsConfirmed)
            {
                await PopupNavigation.Instance.PopAsync();

                viewModel.MccheyneCheckList.ForEach(x =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        x.Ranges[i].IsChecked = false;
                        x.Ranges[i].Color = Color.White;
                        Preferences.Set(x.Ranges[i].Id, x.Ranges[i].IsChecked);
                    }
                });
            }
            
        }

        private async void OnOptionClicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(optionPopup);
        }
    }
}