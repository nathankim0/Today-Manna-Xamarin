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
        private string shareRangeString = "";
        private int _bottomSheetHidedCount = 0;

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
            mannaTextClickSheet.cancelbuttonClicked += OnCancelButtonClicked;

            contentGrid.Children.Add(bottomSheet);
        }

        private async void OnCoppyButtonClicked(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(shareRangeString + "\n" + mannaTextClickSheet.editor.Text);
            await DisplayAlert("클립보드에 복사됨", null, "확인");
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
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

            if (!await DisplayAlert("", "저장하시겠습니까?", "저장", "취소"))
            {
                bottomSheet.Show();
                _bottomSheetHidedCount = 0;
                return;
            }

            var memoItem = new MemoItem
            {
                Verse = shareRangeString,
                Note = mannaTextClickSheet.editor.Text
            };
            await App.Database.SaveItemAsync(memoItem);
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            hideFlag = true;
            bottomSheet.Hide();
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
        private void OnCollectionViewItemTapped(System.Object sender, System.EventArgs e)
        {
            _bottomSheetHidedCount = 0;

            var selectedGrid = sender as Grid;
            SetSelectedItemUnderLined(selectedGrid, true);

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

            var num = ((Label)selectedGrid.Children.ElementAt(0)).Text;
            var manna = ((Label)selectedGrid.Children.ElementAt(1)).Text;

            shareRangeString = $"({tmpRangeString}:{num}) {manna}";

            mannaTextClickSheet.textLabel.Text = shareRangeString;
            mannaTextClickSheet.editor.Text = "";

            bottomSheet.Show();

            bottomSheet.hided += async (s, ee) =>
            {
                if (_bottomSheetHidedCount > 0)
                {
                    SetSelectedItemUnderLined(selectedGrid, false);
                    return;
                }
                _bottomSheetHidedCount++;
                System.Diagnostics.Debug.WriteLine("hided colled!");

                if (mannaTextClickSheet.editor.Text != "")
                {
                    if (!hideFlag)
                    {
                        if (!await DisplayAlert("", "저장하시겠습니까?", "저장", "취소"))
                        {
                            bottomSheet.Show();
                            _bottomSheetHidedCount = 0;
                            return;
                        }
                        else
                        {
                            var memoItem = new MemoItem
                            {
                                Verse = shareRangeString,
                                Note = mannaTextClickSheet.editor.Text
                            };
                            await App.Database.SaveItemAsync(memoItem);

                            SetSelectedItemUnderLined(selectedGrid, false);
                        }
                    }
                }
                else
                {
                    SetSelectedItemUnderLined(selectedGrid, false);
                }
                hideFlag = false;
            };
        }

        private static void SetSelectedItemUnderLined(Grid t, bool isUnderLined)
        {
            if (isUnderLined)
            {
                ((Label)t.Children.ElementAt(0)).TextDecorations = TextDecorations.Underline;
                ((Label)t.Children.ElementAt(1)).TextDecorations = TextDecorations.Underline;
            }
            else
            {
                ((Label)t.Children.ElementAt(0)).TextDecorations = TextDecorations.None;
                ((Label)t.Children.ElementAt(1)).TextDecorations = TextDecorations.None;
            }
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
            mannaViewModel.GetManna(e.NewDate);
        }

        private void OnDatepickerUnfocused(object sender, FocusEventArgs e)
        {
            backgroundBoxView.IsVisible = false;
        }
    }
}
