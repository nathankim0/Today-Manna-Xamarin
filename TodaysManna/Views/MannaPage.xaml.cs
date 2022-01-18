using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using TodaysManna.ViewModel;
using System.Threading.Tasks;
using TodaysManna.Constants;
using System.Diagnostics;
using TodaysManna.Models;
using Rg.Plugins.Popup.Extensions;
using TodaysManna.Controls.Popups;
using TodaysManna.Managers;
using TodaysManna.Views;

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        private string shareRangeString = "";

        public MannaPage()
        {
            InitializeComponent();
            BindingContext = new MannaViewModel();

            Padding = new Thickness(0, Values.StatusBarHeight, 0, 0);

            mannaDatepicker.MinimumDate = new DateTime(DateTime.Now.Year, 1, 1);
            mannaDatepicker.MaximumDate = DateTime.Now;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMannaToTop, (sender) =>
            {
                mannaCollectionView.ScrollTo(0);
            });
        }

        #region toolbar buttons
        private void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("manna_refresh");

            ResetSelectedItemsAndPopPopups();

            mannaDatepicker.MaximumDate = DateTime.Now;

            var now = DateTime.Now;
            mannaDatepicker.Date = now;
            if (mannaDatepicker.Date.Year == now.Year && mannaDatepicker.Date.Month == now.Month && mannaDatepicker.Date.Day == now.Day)
            {
                if (!(BindingContext is MannaViewModel viewModel)) return;
                viewModel.RefreshManna();
            }
        }

        private async void OnEnglishButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("manna_english");

            ResetSelectedItemsAndPopPopups();

            var uri = new Uri(((MannaViewModel)BindingContext)._completeUrl);
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        private void OnMannaDateButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("manna_date");

            ResetSelectedItemsAndPopPopups();

            mannaDatepicker.Focus();
        }

        private async void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.IsRefreshing = true;

            await Task.WhenAll(viewModel.GetManna(e.NewDate));

            viewModel.IsRefreshing = false;

            DependencyService.Get<IHapticFeedback>().Run();
        }
        #endregion toolbar buttons

        private async void OnAllRangeButtonTapped(object sender, EventArgs args)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("manna_range_share");

            ResetSelectedItemsAndPopPopups();

            if (!(BindingContext is MannaViewModel viewModel)) return;

            var shareText = DateTime.Now.ToString("yyyy-MM-dd(dddd)")+"\n"+ "만나: " + viewModel.MannaShareRange + "\n" + "맥체인: " + viewModel.McShareRange;
            await Clipboard.SetTextAsync(shareText);

            await DisplayAlert("클립보드에 복사됨", shareText, "확인");
        }

        private async void OnMannaShareButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("manna_share");

            ResetSelectedItemsAndPopPopups();

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = ((MannaViewModel)BindingContext).Today + "\n\n" + ((MannaViewModel)BindingContext).JsonMannaData.Verse+ "\n\n" + (BindingContext as MannaViewModel).AllString,
                Title = "공유"
            });
        }

        #region CollectionChanged
        //**************************************//
        // Collection View Changed Methods
        //**************************************//
        async void OnMannaCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentView = (CollectionView)sender;
            var seletedItems = e.CurrentSelection;

            if (currentView.SelectedItems.Count > 0 && e.PreviousSelection != null && currentView != null)
            {
                DependencyService.Get<IHapticFeedback>().Run();
            }

            var selectedTexts = "";
            foreach (MannaContent item in seletedItems)
            {
                selectedTexts += $"({item.Verse}) {item.MannaString}\n\n";
            }
            Debug.WriteLine(selectedTexts);
            shareRangeString = selectedTexts;

            if (seletedItems.Count > 0 && PopupNavigation.Instance.PopupStack.Count==0)
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
                try
                {
                    await Navigation.PopAllPopupAsync();
                }
                catch (Exception ex)
                {
                    Debug.Fail($"{this}\n{ex.Message}");
                }
            }
        }
        #endregion CollectionChanged

        private async void OnCopyButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("manna_text_copy");

            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", null, "확인");

            ResetSelectedItemsAndPopPopups();
        }

        private async void OnMemoButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("manna_text_memo");

            var memoPage = new MemoAddPage();
            memoPage.SetBibleText(shareRangeString);
            memoPage.SaveButtonClicked += OnMemoPopupSaveButtonClicked;

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                try
                {
                    await Navigation.PopAllPopupAsync();
                }
                catch (Exception ex)
                {
                    Debug.Fail($"{this}\n{ex.Message}");
                }
            }

            await Navigation.PushAsync(memoPage);
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("manna_text_share");

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

        private async void OnMemoPopupSaveButtonClicked(object sender, (string,string) memoText)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = memoText.Item1,
                Note = memoText.Item2
            };
            await DatabaseManager.Database.SaveItemAsync(memoItem);

            mannaCollectionView.SelectedItems.Clear();
        }

        private async void ResetSelectedItemsAndPopPopups()
        {
            mannaCollectionView.SelectedItems.Clear();
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                try
                {
                    await Navigation.PopAllPopupAsync();
                }
                catch(Exception ex)
                {
                    Debug.Fail($"{this}\n{ex.Message}");
                }
            }
        }
    }
}