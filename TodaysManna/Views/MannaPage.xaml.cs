﻿using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using Rg.Plugins.Popup.Services;
using TodaysManna.ViewModel;
using System.Threading.Tasks;
using TodaysManna.Constants;

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        private readonly BottomSheet _bottomSheet;
        private readonly MannaTextClickSheet _mannaTextClickSheet;

        private readonly MemoPopup _memoPopup;

        private string shareRangeString = "";

        public MannaPage()
        {
            InitializeComponent();
            BindingContext = new MannaViewModel();

            mannaDatepicker.MinimumDate = new DateTime(DateTime.Now.Year, 1, 1);
            mannaDatepicker.MaximumDate = DateTime.Now;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnShareLabelTapped;
            rangeButton.GestureRecognizers.Add(tapGesture);

            _bottomSheet = new BottomSheet();

            _mannaTextClickSheet = new MannaTextClickSheet();
            _bottomSheet.BottomSheetContainer.ContentStackLayout.Children.Add(_mannaTextClickSheet);

            _mannaTextClickSheet.coppybuttonClicked += OnCoppyButtonClicked;
            _mannaTextClickSheet.memobuttonClicked += OnMemoButtonClicked;
            _mannaTextClickSheet.sharebuttonClicked += OnTextShareButtonClicked;
            _mannaTextClickSheet.savebuttonClicked += OnSaveButtonClicked;
            _mannaTextClickSheet.cancelbuttonClicked += OnCancelButtonClicked;

            contentGrid.Children.Add(_bottomSheet);

            _memoPopup = new MemoPopup();
            _memoPopup.SaveButtonClicked += OnMemoPopupSaveButtonClicked;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMannaToTop, (sender) =>
            {
                mannaCollectionView.ScrollTo(0);
            });
        }

        private async void OnMemoButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_text_memo");

            _bottomSheet.Hide();
            _memoPopup.SetBibleText(shareRangeString);
            await PopupNavigation.Instance.PushAsync(_memoPopup);
        }

        private async void OnCoppyButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_text_coppy");

            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", null, "확인");
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

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            _bottomSheet.Hide();

            if (!await DisplayAlert("", "저장하시겠습니까?", "저장", "취소"))
            {
                _bottomSheet.Show();
                return;
            }

            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = shareRangeString,
                Note = ""
            };
            await App.Database.SaveItemAsync(memoItem);
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _bottomSheet.Hide();
        }

        private async void OnShareLabelTapped(object sender, EventArgs args)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_range_share");

            var shareText = DateTime.Now.ToString("yyyy-MM-dd(dddd)")+"\n"+ mannaRangeLabel.Text + "\n" + mcRangeLabel.Text;
            await Clipboard.SetTextAsync(shareText);

            await DisplayAlert("클립보드에 복사됨", shareText, "확인");
        }

        private async void OnShareButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_share");

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = today.Text + "\n\n" + verse.Text + "\n\n" + (BindingContext as MannaViewModel).AllString,
                Title = "공유"
            });
        }

        private async void OnEnglishButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_english");

            var uri = new Uri(((MannaViewModel)BindingContext)._completeUrl);
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        private void OnCollectionViewItemTapped(object sender, EventArgs e)
        {
            var selectedGrid = sender as Grid;

            var verseText = verse.Text;
            string tmpRangeString = "";

            try
            {
                tmpRangeString = verseText.Substring(0, verseText.IndexOf(":"));
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine("OnCollectionViewItemTapped error! : " + error.Message);
            }

            string num = "";
            string manna = "";

            try
            {
                num = ((Label)selectedGrid.Children.ElementAt(0)).FormattedText.Spans[0].Text;
                manna = ((Label)selectedGrid.Children.ElementAt(0)).FormattedText.Spans[2].Text;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }

            shareRangeString = $"({tmpRangeString}:{num}) {manna}\n";
            _mannaTextClickSheet.textLabel.Text = shareRangeString;

            _bottomSheet.Show();
        }

        private void OnMannaDateButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_date");

            backgroundBoxView.IsVisible = true;
            mannaDatepicker.Focus();
        }

        private void OnMannaTodayButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_today");

            mannaDatepicker.Date = DateTime.Now;
        }

        private void OnBackgroundTapped(object sender, EventArgs e)
        {
            backgroundBoxView.IsVisible = false;
            mannaDatepicker.Unfocus();
        }

        private async void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            if (!(BindingContext is MannaViewModel viewModel)) return;

            viewModel.IsRefreshing = true;

            await Task.WhenAll(viewModel.GetManna(e.NewDate));

            viewModel.IsRefreshing = false;
        }

        private void OnDatepickerUnfocused(object sender, FocusEventArgs e)
        {
            backgroundBoxView.IsVisible = false;
        }

        private async void OnMemoPopupSaveButtonClicked(object sender, string memoText)
        {
            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = shareRangeString,
                Note = memoText
            };
            await App.Database.SaveItemAsync(memoItem);
        }

        private void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("manna_refresh");

            mannaDatepicker.MaximumDate = DateTime.Now;

            var now = DateTime.Now;
            mannaDatepicker.Date = now;
            if (mannaDatepicker.Date.Year== now.Year &&  mannaDatepicker.Date.Month == now.Month && mannaDatepicker.Date.Day == now.Day)
            {
                if (!(BindingContext is MannaViewModel viewModel)) return;
                viewModel.RefreshManna();
            }
        }
    }
}