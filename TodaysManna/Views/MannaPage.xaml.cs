using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using System.Linq;

namespace TodaysManna.Views
{
    public partial class MannaPage : ContentPage
    {
        private readonly MannaViewModel mannaViewModel = new MannaViewModel();
        private BottomSheet bottomSheet;
        private MannaTextClickSheet mannaTextClickSheet;

        public MannaPage(/*MannaViewModel mannaViewModel*/)
        {
            InitializeComponent();
            BindingContext = mannaViewModel;

            mannaDatepicker.MinimumDate = new DateTime(DateTime.Now.Year, 1, 1);
            mannaDatepicker.MaximumDate = DateTime.Now;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnShareLabelTapped;
            rangeButton.GestureRecognizers.Add(tapGesture);

            bottomSheet = new BottomSheet();

            mannaTextClickSheet = new MannaTextClickSheet();
            bottomSheet.BottomSheetContainer.ContentStackLayout.Children.Add(mannaTextClickSheet);

            mannaTextClickSheet.coppybuttonClicked += OnCoppyButtonClicked;
            mannaTextClickSheet.sharebuttonClicked += OnTextShareButtonClicked;
            mannaTextClickSheet.savebuttonClicked += OnSaveButtonClicked;

            contentGrid.Children.Add(bottomSheet);
        }


        private async void OnCoppyButtonClicked(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", shareRangeString, "확인");
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = shareRangeString,
                Title = "공유"
            });
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {

        }


        private async void OnShareLabelTapped(object sender, EventArgs args)
        {
            await rangeButton.ScaleTo(0.8, 150);
            var shareText = allRangeLabel.Text;
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
        string shareRangeString = "";
        private async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var t = sender as Grid;

            ((Label)t.Children.ElementAt(0)).TextDecorations = TextDecorations.Underline;
            ((Label)t.Children.ElementAt(1)).TextDecorations = TextDecorations.Underline;

            var verseText = verse.Text;

            string tmpRangeString = "";
            try
            {
                tmpRangeString = verseText.Substring(0, verseText.IndexOf(":"));
            }
            catch (NullReferenceException error)
            {
                System.Diagnostics.Debug.WriteLine(error.Message);
            }
            var num = ((Label)t.Children.ElementAt(0)).Text;
            var manna = ((Label)t.Children.ElementAt(1)).Text;

            shareRangeString = $"({tmpRangeString}:{num}) {manna}";

            //await Clipboard.SetTextAsync(shareRangeString);
            //await DisplayAlert("클립보드에 복사됨", shareRangeString, "확인");

            mannaTextClickSheet.textLabel.Text = shareRangeString;

            bottomSheet.Show();

            bottomSheet.hided += (s, ee) =>
            {
                ((Label)t.Children.ElementAt(0)).TextDecorations = TextDecorations.None;
                ((Label)t.Children.ElementAt(1)).TextDecorations = TextDecorations.None;
            };
        }


        private void OnMannaDateButtonClicked(object sender, EventArgs e)
        {
            backgroundBoxView.IsVisible = true;
            mannaDatepicker.Focus();
        }

        void mannaDatepicker_DateSelected(System.Object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            mannaViewModel.GetManna(e.NewDate);
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

        void datepicker_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            backgroundBoxView.IsVisible = false;
        }
    }
}
