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

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        private readonly MemoPopup _memoPopup;
        private string shareRangeString = "";

        private double headerHeight;

        public MannaPage()
        {
            InitializeComponent();
            BindingContext = new MannaViewModel();

            mannaDatepicker.MinimumDate = new DateTime(DateTime.Now.Year, 1, 1);
            mannaDatepicker.MaximumDate = DateTime.Now;

            _memoPopup = new MemoPopup();
            _memoPopup.SaveButtonClicked += OnMemoPopupSaveButtonClicked;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMannaToTop, (sender) =>
            {
                mannaCollectionView.ScrollTo(0);
            });

            headerStackLayout.SizeChanged += HeaderStackLayout_SizeChanged;
        }

        private void HeaderStackLayout_SizeChanged(object sender, EventArgs e)
        {
            headerStackLayout.SizeChanged -= HeaderStackLayout_SizeChanged;
            headerHeight = headerStackLayout.Height;
            rangeButton.Margin = new Thickness(0, headerHeight, 0, 0);
        }

        #region toolbar buttons
        private void OnRefreshButtonClicked(object sender, EventArgs e)
        {
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
            FirebaseEventService.SendEventOnPlatformSpecific("manna_english");

            ResetSelectedItemsAndPopPopups();

            var uri = new Uri(((MannaViewModel)BindingContext)._completeUrl);
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        private void OnMannaDateButtonClicked(object sender, EventArgs e)
        {
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
        }
        #endregion toolbar buttons

        private async void OnAllRangeButtonTapped(object sender, EventArgs args)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_range_share");

            ResetSelectedItemsAndPopPopups();

            var shareText = DateTime.Now.ToString("yyyy-MM-dd(dddd)")+"\n"+ mannaRangeLabel.Text + "\n" + mcRangeLabel.Text;
            await Clipboard.SetTextAsync(shareText);

            await DisplayAlert("클립보드에 복사됨", shareText, "확인");
        }

        private async void OnMannaShareButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_share");

            ResetSelectedItemsAndPopPopups();

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = ((MannaViewModel)BindingContext).Today + "\n\n" + ((MannaViewModel)BindingContext).JsonMannaData.Verse+ "\n\n" + (BindingContext as MannaViewModel).AllString,
                Title = "공유"
            });
        }

        async void OnMannaCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var seletedItems = e.CurrentSelection;

            //if(e.PreviousSelection == null)
            //{
            //    return;
            //}
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
            else if(seletedItems.Count == 0 && PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await Navigation.PopAllPopupAsync();
            }
        }

        private async void OnCopyButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_text_copy");

            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", null, "확인");

            ResetSelectedItemsAndPopPopups();
        }

        private async void OnMemoButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_text_memo");

            _memoPopup.SetBibleText(shareRangeString);
            await PopupNavigation.Instance.PushAsync(_memoPopup);
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_text_share");

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = shareRangeString,
                Title = "공유"
            });
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            ResetSelectedItemsAndPopPopups();
        }

        //private async void OnSaveButtonClicked(object sender, EventArgs e)
        //{
        //    // _bottomSheet.Hide();

        //    if (!await DisplayAlert("", "저장하시겠습니까?", "저장", "취소"))
        //    {
        //        // _bottomSheet.Show();
        //        return;
        //    }

        //    var memoItem = new MemoItem
        //    {
        //        Date = DateTime.Now,
        //        Verse = shareRangeString,
        //        Note = ""
        //    };
        //    await App.Database.SaveItemAsync(memoItem);
        //}

        private async void OnMemoPopupSaveButtonClicked(object sender, string memoText)
        {
            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = shareRangeString,
                Note = memoText
            };
            await App.Database.SaveItemAsync(memoItem);

            ResetSelectedItemsAndPopPopups();
        }

        private async void ResetSelectedItemsAndPopPopups()
        {
            mannaCollectionView.SelectedItems.Clear();
            await Navigation.PopAllPopupAsync();
        }

        private double previousScrollPosition = 0;
        void mannaCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (previousScrollPosition < e.VerticalOffset)
            {
                Debug.WriteLine("scrolled down");
                previousScrollPosition = e.VerticalOffset;
                headerStackLayout.TranslateTo(0, -150, 250, Easing.CubicOut);
            }
            else
            {
                Debug.WriteLine("scrolled up");
                headerStackLayout.TranslateTo(0, 0, 250, Easing.CubicOut);
            }
            previousScrollPosition = e.VerticalOffset;
        }
    }
}