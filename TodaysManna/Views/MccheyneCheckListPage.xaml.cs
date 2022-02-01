using Xamarin.Forms;
using System.Linq;
using System;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using TodaysManna.ViewModel;
using TodaysManna.Models;
using System.Diagnostics;

namespace TodaysManna
{
    public partial class MccheyneCheckListPage : ContentPage
    {
        private readonly OptionPopup _optionPopup;
        private MccheyneCheckListContent _todayMccheyne;

        public MccheyneCheckListPage()
        {
            InitializeComponent();

            Padding = new Thickness(0, Constants.StatusBarHeight, 0, 0);

            var mccheyneCheckViewModel = new MccheyneCheckViewModel();
            BindingContext = mccheyneCheckViewModel;

            _todayMccheyne = mccheyneCheckViewModel.MccheyneCheckList.Where(x => x.Date == DateTime.Now.ToString("M-d")).FirstOrDefault();

            _optionPopup = new OptionPopup();
            _optionPopup.CheckButtonClicked += OnCheckButtonClicked;
            _optionPopup.ClearButtonClicked += OnClearButtonClicked;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollCheckListToTop, (sender) =>
            {
                ScrollToToday(true);
            });
        }

        protected override void OnAppearing()
        {
            if (Constants.IsDeviceIOS)
            {
                CustomOnAppearing();
            }
            else
            {
                if (Constants.MccheynePageLaunchCount >= 2)
                {
                    CustomOnAppearing();
                }
                else
                {
                    Constants.MccheynePageLaunchCount++;
                }
            }
        }

        public void CustomOnAppearing()
        {
            ScrollToToday(false);
        }

        private void OnScrollToToday(object sender, EventArgs e)
        {
            ScrollToToday(false);
        }

        private async void OnCheckButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
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
                    }
                });
                ScrollToToday(false);
            }
        }

        private async void OnClearButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
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
                        Preferences.Set(x.Ranges[i].Id, x.Ranges[i].IsChecked);
                    }
                });
            }

        }

        private async void OnOptionClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            await PopupNavigation.Instance.PushAsync(_optionPopup);
        }

        private void OnMccheyneCheckTapped(object sender, EventArgs e)
        {
            var mccheyneOneRange = ((TappedEventArgs)e).Parameter as MccheyneOneRange;
            mccheyneOneRange.IsChecked = !mccheyneOneRange.IsChecked;
        }

        private void OnDateTapped(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("checklist_go_to_read");

            var mccheyneCheckListContent = ((TappedEventArgs)e).Parameter as MccheyneCheckListContent;
            var date = mccheyneCheckListContent.Date;

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

        private void ScrollToToday(bool isAnimationEnabled)
        {
            collectionView.ScrollTo(_todayMccheyne, null, ScrollToPosition.Center, isAnimationEnabled);
        }
    }
}
