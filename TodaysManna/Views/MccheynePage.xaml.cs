using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using TodaysManna.ViewModel;
using System.Threading.Tasks;

using TodaysManna.Models;
using System.Diagnostics;
using TodaysManna.Controls.Popups;
using Rg.Plugins.Popup.Extensions;
using TodaysManna.Managers;
using TodaysManna.Views;

namespace TodaysManna
{
    public partial class MccheynePage : ContentPage
    {
        //private readonly MemoPopup _memoPopup;
        private string shareRangeString = "";
        public MccheynePage()
        {
            InitializeComponent();
            Padding = new Thickness(0, Constants.StatusBarHeight, 0, 0);
            BindingContext = new MccheyneViewModel();

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMccheyneToTop, (sender) =>
            {
                mccheyneCollectionView.ScrollTo(0);
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!(BindingContext is MccheyneViewModel viewModel)) return;
            viewModel.SetTodayCheckList();
        }

        private void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_today");

            ResetSelectedItemsAndPopPopups();

            datepicker.Date = DateTime.Now;
        }

        private void OnDateButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_date");

            ResetSelectedItemsAndPopPopups();

            datepicker.Focus();
        }

        private async void OnDatePickerDateSelected(object sender, DateChangedEventArgs e)
        {
            if (!(BindingContext is MccheyneViewModel viewModel)) return;

            viewModel.Today = e.NewDate.ToString("M_d");
            viewModel.CurrentSettedDateTime = MccheyneViewModel.GetCorrectDateLeapYear(e.NewDate);

            viewModel.IsRefreshing = true;

            var task1 = viewModel.GetMccheyne();
            var task2 = viewModel.GetMccheyneRange();
            await Task.WhenAll(task1, task2);

            viewModel.SetTodayCheckList();

            viewModel.IsRefreshing = false;

            DependencyService.Get<IHapticFeedback>().Run();
        }


        #region CollectionChanged
        //**************************************//
        // Collection View Changed Methods
        //**************************************//
        async void OnMccheyneCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentView = (CollectionView)sender;
            var seletedItems = e.CurrentSelection;

            if (currentView.SelectedItems.Count > 0 && e.PreviousSelection != null && currentView != null)
            {
                DependencyService.Get<IHapticFeedback>().Run();
            }

            var selectedTexts = "";
            foreach (MccheyneContent item in seletedItems)
            {
                selectedTexts += $"({item.Book}{item.Verse}) {item.Content}\n\n";
            }
            Debug.WriteLine(selectedTexts);
            shareRangeString = selectedTexts;

            if (seletedItems.Count > 0 && PopupNavigation.Instance.PopupStack.Count == 0)
            {
                var popup = new SelectFeaturePopup();
                popup.CopybuttonClicked += OnCopyButtonClicked;
                popup.MemobuttonClicked += OnMemoButtonClicked;
                popup.SharebuttonClicked += OnTextShareButtonClicked;
                popup.CancelbuttonClicked += OnCancelButtonClicked;

                await Navigation.PushPopupAsync(popup);
            }
            else if (seletedItems.Count == 0 && PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await Navigation.PopAllPopupAsync();
            }
        }
        #endregion CollectionChanged


        private async void OnCopyButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_text_coppy");

            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", null, "확인");

            ResetSelectedItemsAndPopPopups();
        }

        private async void OnMemoButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_text_memo");

            var memoPage = new MemoAddPage();
            memoPage.SetBibleText(shareRangeString);
            memoPage.SaveButtonClicked += OnMemoPopupSaveButtonClicked;

            ResetSelectedItemsAndPopPopups();

            await Navigation.PushAsync(memoPage);
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_text_share");

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = shareRangeString,
                Title = "공유"
            });
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            ResetSelectedItemsAndPopPopups();
        }

        private async void OnMemoPopupSaveButtonClicked(object sender, (string, string) memoText)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = memoText.Item1,
                Note = memoText.Item2
            };
            await DatabaseManager.Database.SaveItemAsync(memoItem);
        }

        private async void ResetSelectedItemsAndPopPopups()
        {
            try
            {
                mccheyneCollectionView.SelectedItems.Clear();
                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    await Navigation.PopAllPopupAsync();
                }
            }
            catch
            {

            }
           
        }

        private void OnRange1Tapped(object sender, EventArgs args)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            if (!(BindingContext is MccheyneViewModel viewModel)) return;

            viewModel.IsRange1Selected = true;
            viewModel.IsRange2Selected = false;
            viewModel.IsRange3Selected = false;
            viewModel.IsRange4Selected = false;

            mccheyneCollectionView.ItemsSource = viewModel.MccheyneContents1;
        }

        private void OnRange2Tapped(object sender, EventArgs args)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            if (!(BindingContext is MccheyneViewModel viewModel)) return;

            viewModel.IsRange1Selected = false;
            viewModel.IsRange2Selected = true;
            viewModel.IsRange3Selected = false;
            viewModel.IsRange4Selected = false;

            mccheyneCollectionView.ItemsSource = viewModel.MccheyneContents2;
        }

        private void OnRange3Tapped(object sender, EventArgs args)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            if (!(BindingContext is MccheyneViewModel viewModel)) return;

            viewModel.IsRange1Selected = false;
            viewModel.IsRange2Selected = false;
            viewModel.IsRange3Selected = true;
            viewModel.IsRange4Selected = false;

            mccheyneCollectionView.ItemsSource = viewModel.MccheyneContents3;
        }

        private void OnRange4Tapped(object sender, EventArgs args)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            if (!(BindingContext is MccheyneViewModel viewModel)) return;

            viewModel.IsRange1Selected = false;
            viewModel.IsRange2Selected = false;
            viewModel.IsRange3Selected = false;
            viewModel.IsRange4Selected = true;

            mccheyneCollectionView.ItemsSource = viewModel.MccheyneContents4;
        }

        void OnMccheyneCheckTapped(object sender, EventArgs e)
        {
            var mccheyneOneRange = ((TappedEventArgs)e).Parameter as MccheyneOneRange;
            mccheyneOneRange.IsChecked = !mccheyneOneRange.IsChecked;
        }

        //double previousScrollPosition = 0;
        //void mccheyneCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        //{
        //    if (previousScrollPosition < e.VerticalOffset)
        //    {
        //        mccheynToggle.TranslateTo(0, 70, 250u, Easing.CubicOut);
        //        mccheynToggle.FadeTo(0, 150);

        //        previousScrollPosition = e.VerticalOffset;
        //    }
        //    else if (previousScrollPosition > e.VerticalOffset)
        //    {
        //        mccheynToggle.Opacity = 1;
        //        mccheynToggle.TranslateTo(0, 0, 200u, Easing.CubicOut);
        //    }
        //    previousScrollPosition = e.VerticalOffset;
        //}
    }
}
