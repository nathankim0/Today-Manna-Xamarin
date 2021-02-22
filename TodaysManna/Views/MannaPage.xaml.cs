using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using System.Linq;
using TodaysManna.Models;
using TodaysManna.Popups;
using Rg.Plugins.Popup.Services;

namespace TodaysManna.Views
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
        }
        
        private async void OnMemoButtonClicked(object sender, EventArgs e)
        {
            _bottomSheet.Hide();
            _memoPopup.SetBibleText(shareRangeString);
            await PopupNavigation.Instance.PushAsync(_memoPopup);
            
        }

        private async void OnCoppyButtonClicked(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", null, "확인");
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
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
                Verse = shareRangeString,
                Note =""
            };
            await App.Database.SaveItemAsync(memoItem);
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            _bottomSheet.Hide();
        }

        private async void OnShareLabelTapped(object sender, EventArgs args)
        {
            await rangeButton.ScaleTo(0.8, 150);

            var shareText = mannaRangeLabel.Text + "\n" + mcRangeLabel.Text;
            await Clipboard.SetTextAsync(shareText);
            await DisplayAlert("클립보드에 복사됨", shareText, "확인");

            await rangeButton.ScaleTo(1, 150);
        }

        private async void OnShareButtonClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = today.Text + "\n\n" + verse.Text + "\n\n" + (BindingContext as MannaViewModel).AllString,
                Title = "공유"
            });
        }

        private async void OnEnglishButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var uri = new Uri(((MannaViewModel)BindingContext)._completeAppUrl);
                await Browser.OpenAsync(uri, BrowserLaunchMode.External);
            }
            catch
            {
                var uri = new Uri(((MannaViewModel)BindingContext)._completeUrl);
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }   
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
                num = ((Label)selectedGrid.Children.ElementAt(0)).Text;
                manna = ((Label)selectedGrid.Children.ElementAt(1)).Text;
            }
            catch (Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }

            shareRangeString = $"({tmpRangeString}:{num}) {manna}";
            _mannaTextClickSheet.textLabel.Text = shareRangeString;

            _bottomSheet.Show();
        }

        private void OnMannaDateButtonClicked(object sender, EventArgs e)
        {
            backgroundBoxView.IsVisible = true;
            mannaDatepicker.Focus();
        }
        private void OnMannaTodayButtonClicked(object sender, EventArgs e)
        {
            mannaDatepicker.Date = DateTime.Now;
        }

        private void OnBackgroundTapped(object sender, EventArgs e)
        {
            backgroundBoxView.IsVisible = false;
            mannaDatepicker.Unfocus();
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as MannaViewModel).GetManna(e.NewDate);
        }

        private void OnDatepickerUnfocused(object sender, FocusEventArgs e)
        {
            backgroundBoxView.IsVisible = false;
        }

        private async void OnMemoPopupSaveButtonClicked(object sender, string memoText)
        {
            if (!await DisplayAlert("", "저장하시겠습니까?", "저장", "취소"))
            {
                return;
            }

            var memoItem = new MemoItem
            {
                Verse = shareRangeString,
                Note = memoText
            };
            await App.Database.SaveItemAsync(memoItem);
        }
    }
}




//private static void SetSelectedItemUnderLined(Grid t, bool isUnderLined)
//{
//    if (isUnderLined)
//    {
//        ((Label)t.Children.ElementAt(0)).TextDecorations = TextDecorations.Underline;
//        ((Label)t.Children.ElementAt(1)).TextDecorations = TextDecorations.Underline;
//    }
//    else
//    {
//        ((Label)t.Children.ElementAt(0)).TextDecorations = TextDecorations.None;
//        ((Label)t.Children.ElementAt(1)).TextDecorations = TextDecorations.None;
//    }
//}
