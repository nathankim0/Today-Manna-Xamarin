using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using UIKit;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TodaysManna.Views
{
    public partial class MannaPage : ContentPage
    {
        private readonly MannaViewModel mannaViewModel = new MannaViewModel();
        //private static readonly MyPopupPage popupPage = new MyPopupPage();

        public MannaPage()
        {
            InitializeComponent();

            BindingContext = mannaViewModel;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnShareLabelTapped;
            rangeButton.GestureRecognizers.Add(tapGesture);

            // UIApplication.SharedApplication.ApplicationIconBadgeNumber = -1;
        }

        private async void OnShareLabelTapped(object sender, EventArgs args)
        {
            //  await PopupNavigation.Instance.PushAsync(popupPage);

            await rangeButton.ScaleTo(0.8, 150);
            var shareText = allRangeLabel.Text;
            await Clipboard.SetTextAsync(shareText);
            await DisplayAlert("클립보드에 복사됨", shareText, "확인");

            await rangeButton.ScaleTo(1, 150);

            //await Share.RequestAsync(new ShareTextRequest
            //{
            //    Text = allRangeLabel.Text,
            //    Title = "공유"
            //});
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

            var shareRangeString = $"({tmpRangeString}:{num}){manna}";

            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", shareRangeString, "확인");

            //await Share.RequestAsync(new ShareTextRequest
            //{
            //    Text = shareRangeString,
            //    Title = "공유"
            //});
            ((Label)t.Children.ElementAt(0)).TextDecorations = TextDecorations.None;
            ((Label)t.Children.ElementAt(1)).TextDecorations = TextDecorations.None;
        }

        //private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
        //{
        //    var view = sender as CollectionView;
        //    if (view.SelectedItem == null) return;
        //    if (e.CurrentSelection.FirstOrDefault() == null) return;

        //    var manna = e.CurrentSelection.FirstOrDefault() as MannaContent;

        //    var verseText = verse.Text;

        //    string tmpRangeString = "";
        //    try
        //    {
        //        tmpRangeString = verseText.Substring(0, verseText.IndexOf(":"));
        //    }
        //    catch(NullReferenceException error)
        //    {
        //        System.Diagnostics.Debug.WriteLine(error.Message);
        //    }


        //    var shareRangeString = $"({tmpRangeString}:{manna.Number}){manna.MannaString}";

        //    //await Share.RequestAsync(new ShareTextRequest
        //    //{
        //    //    Text = shareRangeString,
        //    //    Title = "공유"
        //    //});
        //    await Clipboard.SetTextAsync(shareRangeString);
        //    await DisplayAlert("클립보드 복사됨", null, "확인");

        //    if (view.SelectedItem != null)
        //    {
        //        view.SelectedItem = null;
        //    }
        //}
    }
}
