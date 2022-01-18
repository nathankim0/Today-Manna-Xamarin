using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Services;
using TodaysManna.ViewModel;
using System.Threading.Tasks;
using TodaysManna.Constants;
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
        private CollectionView currentView;
        private double headerHeight;

        public MccheynePage()
        {
            InitializeComponent();

            Padding = new Thickness(0, Values.StatusBarHeight, 0, 0);

            BindingContext = new MccheyneViewModel();

            //_memoPopup = new MemoPopup();
            //_memoPopup.SaveButtonClicked += OnMemoPopupSaveButtonClicked;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMccheyneToTop, (sender) =>
            {
                mccheyneCollectionView.ScrollTo(0);
            });
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
            var thisDate = MccheyneViewModel.GetCorrectDateLeapYear(e.NewDate);

            viewModel.IsRefreshing = true;

            var task1 = viewModel.GetMccheyne(thisDate);
            var task2 = viewModel.GetMccheyneRange(thisDate);
            await Task.WhenAll(task1, task2);

            viewModel.IsRefreshing = false;

            DependencyService.Get<IHapticFeedback>().Run();
        }


        #region CollectionChanged
        //**************************************//
        // Collection View Changed Methods
        //**************************************//
        async void OnMccheyneCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentView = (CollectionView)sender;
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
                var popup = new SelectionChangedPopup();
                popup.copybuttonClicked += OnCopyButtonClicked;
                popup.memobuttonClicked += OnMemoButtonClicked;
                popup.sharebuttonClicked += OnTextShareButtonClicked;
                popup.cancelbuttonClicked += OnCancelButtonClicked;

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

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await Navigation.PopAllPopupAsync();
            }

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

        private async void OnMemoPopupSaveButtonClicked(object sender, string memoText)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = shareRangeString,
                Note = memoText
            };
            await DatabaseManager.Database.SaveItemAsync(memoItem);
            if (currentView != null)
            {
                currentView.SelectedItems.Clear();
                currentView = null;
            }
        }

        private async void ResetSelectedItemsAndPopPopups()
        {
            if (currentView != null)
            {
                currentView.SelectedItems.Clear();
                currentView = null;
            }
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await Navigation.PopAllPopupAsync();
            }
        }

        #region Toggle Tapped Events
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
        #endregion

    }
}
