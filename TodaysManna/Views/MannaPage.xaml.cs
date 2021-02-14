using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using System.Linq;
using TodaysManna.Models;

namespace TodaysManna.Views
{
    public partial class MannaPage : ContentPage
    {
        private readonly MannaViewModel mannaViewModel = new MannaViewModel();
        private BottomSheet bottomSheet;
        private MannaTextClickSheet mannaTextClickSheet;

        private bool hideFlag = false;

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
            hideFlag = true;
            bottomSheet.Hide();
            await Clipboard.SetTextAsync(shareRangeString + "\n" + mannaTextClickSheet.editor.Text);
            await DisplayAlert("클립보드에 복사됨", null, "확인");
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
            hideFlag = true;
            bottomSheet.Hide();

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = shareRangeString + "\n" + mannaTextClickSheet.editor.Text,
                Title = "공유"
            });
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            hideFlag = true;
            bottomSheet.Hide();

            if (!await DisplayAlert("저장", "저장하시겠습니까? (취소하면 작성 중인 메모는 사라집니다)", "저장", "취소")) { return; }

            var memoItem = new MemoItem
            {
                Verse = shareRangeString,
                Note = mannaTextClickSheet.editor.Text
            };
            await App.Database.SaveItemAsync(memoItem);
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

        string shareRangeString = "";

        //public void OnCollectionViewSelected(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.CurrentSelection == null) { return; }

        //    var manna = e.CurrentSelection.FirstOrDefault() as MannaContent;
        //    shareRangeString = $"({manna.Verse}) {manna.MannaString}";

        //    mannaTextClickSheet.textLabel.Text = shareRangeString;
        //    mannaTextClickSheet.editor.Text = "";

        //    bottomSheet.Show();

        //    ((CollectionView)sender).SelectedItem = null;
        //}
        int cnt = 0;
        private void OnCollectionViewItemTapped(System.Object sender, System.EventArgs e)
        {
            cnt = 0;

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

            mannaTextClickSheet.textLabel.Text = shareRangeString;
            mannaTextClickSheet.editor.Text = "";

            bottomSheet.Show();
            
            bottomSheet.hided += async (s, ee) =>
            {
                if (cnt > 0) return;
                cnt++;
                System.Diagnostics.Debug.WriteLine("hided colled!");


                if (mannaTextClickSheet.editor.Text != "" && !hideFlag)
                {
                    if (!await DisplayAlert("경고", "저장 버튼을 누르지 않으면 작성 중인 메모가 사라집니다", "확인", "취소"))
                    {
                        bottomSheet.Show();
                        cnt = 0;
                        return;
                    }
                    else
                    {
                        ((Label)t.Children.ElementAt(0)).TextDecorations = TextDecorations.None;
                        ((Label)t.Children.ElementAt(1)).TextDecorations = TextDecorations.None;
                    }
                }
                else
                {
                    ((Label)t.Children.ElementAt(0)).TextDecorations = TextDecorations.None;
                    ((Label)t.Children.ElementAt(1)).TextDecorations = TextDecorations.None;
                }
                hideFlag = false;
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
