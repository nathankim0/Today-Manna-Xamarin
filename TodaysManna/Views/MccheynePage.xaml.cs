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
using Xamarin.CommunityToolkit.UI.Views;
using TodaysManna.Managers;

namespace TodaysManna
{
    public partial class MccheynePage : ContentPage
    {
        private readonly MemoPopup _memoPopup;
        private string shareRangeString = "";
        private CollectionView currentView;
        private double headerHeight;

        public MccheynePage()
        {
            InitializeComponent();
            BindingContext = new MccheyneViewModel();

            _memoPopup = new MemoPopup();
            _memoPopup.SaveButtonClicked += OnMemoPopupSaveButtonClicked;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMccheyneToTop, (sender) =>
            {
                mccheyneCollectionView1.ScrollTo(0);
                mccheyneCollectionView2.ScrollTo(0);
                mccheyneCollectionView3.ScrollTo(0);
                mccheyneCollectionView4.ScrollTo(0);

                headerStackLayout.TranslationY = 0;
                tabView.TranslationY = 0;
            });
            headerStackLayout.SizeChanged += HeaderStackLayout_SizeChanged;
        }

        private void HeaderStackLayout_SizeChanged(object sender, EventArgs e)
        {
            headerStackLayout.SizeChanged -= HeaderStackLayout_SizeChanged;
            headerHeight = headerStackLayout.Height;
            tabView.Margin = new Thickness(0, headerHeight, 0, -headerHeight);
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

        void TabView_SelectionChanged(object sender, TabSelectionChangedEventArgs e)
        {
            headerStackLayout.TranslateTo(0, 0, 250, Easing.CubicOut);
            tabView.TranslateTo(0, 0, 250, Easing.CubicOut);
            ResetSelectedItemsAndPopPopups();
        }

        void TabViewItem_TabTapped(object sender, TabTappedEventArgs e)
        {
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

            _memoPopup.SetBibleText(shareRangeString);
            await PopupNavigation.Instance.PushAsync(_memoPopup);
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

            ResetSelectedItemsAndPopPopups();
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

        private double previousScrollPosition = 0;
        void OnMccheyneCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    if (previousScrollPosition < e.VerticalOffset)
                    {
                        Debug.WriteLine("scrolled down");
                        headerStackLayout.TranslateTo(0, -150, 250, Easing.CubicOut);
                        tabView.TranslateTo(0, -headerHeight, 250, Easing.CubicOut);
                    }
                    else
                    {
                        Debug.WriteLine("scrolled up");
                        headerStackLayout.TranslateTo(0, 0, 250, Easing.CubicOut);
                        tabView.TranslateTo(0, 0, 250, Easing.CubicOut);
                    }
                    previousScrollPosition = e.VerticalOffset;
                    break;
                case Device.Android:
                    if (e.FirstVisibleItemIndex > 0)
                    {
                        new Animation
                        {
                            { 0, 1, new Animation (v => headerStackLayout.TranslationY = v, headerStackLayout.TranslationY, -150) },
                            { 0, 1, new Animation (v => tabView.TranslationY = v, tabView.TranslationY, -headerHeight) }
                            }.Commit(this, "animation", 16, 250, null);
                    }
                    else
                    {
                        new Animation
                        {
                            { 0, 1, new Animation (v => headerStackLayout.TranslationY = v, headerStackLayout.TranslationY, 0) },
                            { 0, 1, new Animation (v => tabView.TranslationY = v, tabView.TranslationY, 0) }
                            }.Commit(this, "animation", 16, 250, null);
                    }

                    //if (previousScrollPosition < e.VerticalOffset)
                    //{
                    //    Debug.WriteLine("scrolled down");
                    //    headerStackLayout.TranslationY = -150;
                    //    tabView.TranslationY = -headerHeight;
                    //    //                    new Animation {
                    //    //{ 0, 1, new Animation (v => headerStackLayout.TranslationY = v, headerStackLayout.TranslationY, -150) },
                    //    //{ 0, 1, new Animation (v => tabView.TranslationY = v, tabView.TranslationY, -headerHeight) }
                    //    //}.Commit(this, "animation", 16, 250, null);
                    //}
                    //else
                    //{
                    //    Debug.WriteLine("scrolled up");
                    //    headerStackLayout.TranslationY = 0;
                    //    tabView.TranslationY = 0;
                    //    //                    new Animation {
                    //    //{ 0, 1, new Animation (v => headerStackLayout.TranslationY = v, headerStackLayout.TranslationY, 0) },
                    //    //{ 0, 1, new Animation (v => tabView.TranslationY = v, tabView.TranslationY, 0) }
                    //    //}.Commit(this, "animation", 16, 250, null);
                    //}
                    //previousScrollPosition = e.VerticalOffset;

                        break;
                case Device.macOS:
                case Device.UWP:
                    break;
                default:
                    break;

            }
        }
    }
}
