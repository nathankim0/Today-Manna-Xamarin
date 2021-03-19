using System;
using TodaysManna.ViewModel;
using Xamarin.Forms;
using System.Linq;
using TodaysManna.Models;
using Xamarin.Essentials;
using ListView = Xamarin.Forms.ListView;
using TodaysManna.Popups;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;

namespace TodaysManna.Views
{
    public partial class MccheynePage : ContentPage
    {
        private readonly BottomSheet _bottomSheet;
        private readonly MannaTextClickSheet _mannaTextClickSheet;
        private readonly MemoPopup _memoPopup;

        private string shareRangeString = "";

        private readonly double gridX;
        private readonly double gridY;

        //private readonly double titleX;
        //private readonly double titleY;


        private int flag = 1;
        private double previousScrollPosition = 0;

        public MccheynePage()
        {
            InitializeComponent();
            BindingContext = new MccheyneViewModel();

            gridX = bottomGrid.TranslationX;
            gridY = bottomGrid.TranslationY;

            //titleX = TitleLayout.TranslationX;
            //titleY = TitleLayout.TranslationY;

            var leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            var rightSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };

            leftSwipeGesture.Swiped += OnLeftSwiped;
            rightSwipeGesture.Swiped += OnRightSwiped;

            mccheyneView.GestureRecognizers.Add(leftSwipeGesture);
            mccheyneView.GestureRecognizers.Add(rightSwipeGesture);

            _bottomSheet = new BottomSheet();
            _mannaTextClickSheet = new MannaTextClickSheet();

            _bottomSheet.BottomSheetContainer.ContentStackLayout.Children.Add(_mannaTextClickSheet);

            _mannaTextClickSheet.coppybuttonClicked += OnCoppyButtonClicked;
            _mannaTextClickSheet.memobuttonClicked += OnMemoButtonClicked;
            _mannaTextClickSheet.sharebuttonClicked += OnTextShareButtonClicked;
            _mannaTextClickSheet.savebuttonClicked += OnSaveButtonClicked;
            _mannaTextClickSheet.cancelbuttonClicked += OnCancelButtonClicked;

            mccheynGrid.Children.Add(_bottomSheet);

            _memoPopup = new MemoPopup();
            _memoPopup.SaveButtonClicked += OnSaveButtonClicked;
        }

        private void PageToLeft()
        {
            if (flag == 1)
            {
            }
            else if (flag == 2) // 2->1
            {
                centerLocationLabel.Text = "1/4";
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents1");
                flag = 1;
                leftImageButton.IsVisible = false;
                rightImageButton.IsVisible = true;
            }
            else if (flag == 3) // 3->2
            {
                centerLocationLabel.Text = "2/4";
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
                flag = 2;
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = true;
            }
            else if (flag == 4) // 4->3
            {
                centerLocationLabel.Text = "3/4";
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
                flag = 3;
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = true;
            }
            mccheyneView.ScrollTo(mccheyneView.ItemsSource.Cast<object>().FirstOrDefault(), ScrollToPosition.End, false);
        }

        private void PageToRight()
        {
            if (flag == 1)
            {
                centerLocationLabel.Text = "2/4";
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
                flag = 2;
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = true;
            }
            else if (flag == 2)
            {
                centerLocationLabel.Text = "3/4";
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = true;
                flag = 3;
            }
            else if (flag == 3)
            {
                centerLocationLabel.Text = "4/4";
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents4");
                flag = 4;
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = false;
            }
            else if (flag == 4)
            {
            }
            mccheyneView.ScrollTo(mccheyneView.ItemsSource.Cast<object>().FirstOrDefault(), ScrollToPosition.End, false);
        }
        private void OnRightButtonClicked(object sender, EventArgs e)
        {
            PageToRight();
        }

        private void OnLeftButtonClicked(object sender, EventArgs e)
        {
            PageToLeft();
        }

        private void OnRightSwiped(object sender, SwipedEventArgs e)
        {
            PageToLeft();
        }

        private void OnLeftSwiped(object sender, SwipedEventArgs e)
        {
            PageToRight();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents1");
            flag = 1;
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
            flag = 2;
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
            flag = 3;
        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents4");
            flag = 4;
        }

        private void OnDatePickerDateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as MccheyneViewModel).Today = e.NewDate.ToString("M_d");
            DateTime thisDate = MccheyneViewModel.GetCorrectDateLeapYear(e.NewDate);

            (BindingContext as MccheyneViewModel).GetMccheyne(thisDate);
            (BindingContext as MccheyneViewModel).GetMccheyneRange(thisDate);
        }

        private void OnTodayButtonClicked(object sender, EventArgs e)
        {
            FirebaseEvent.eventTracker.SendEvent("mccheyn_today");

            datepicker.Date = DateTime.Now;
        }

        private void OnDateButtonClicked(object sender, EventArgs e)
        {
            FirebaseEvent.eventTracker.SendEvent("mccheyn_date");

            backgroundBoxView.IsVisible = true;
            datepicker.Focus();
        }

        private void OnListViewScrolled(object sender, ScrolledEventArgs e)
        {

            if (previousScrollPosition < e.ScrollY) //down
            {
                //var one = TitleLayout.TranslateTo(titleX, titleY - 100, 200u, Easing.CubicOut);
                //var two = TitleLayout.FadeTo(0, 150);

                //new Animation
                //{
                //    {
                //        0, 0.5, new Animation(v => TitleLayout.TranslationY = v,titleY,titleY-100,
                //            Easing.CubicOut)
                //    },
                //    {
                //        0, 1, new Animation(v => TitleLayout.Opacity = v,
                //            1,
                //            0)
                //    }
                //}.Commit(this, "VisiblePicker", 10, 250u, null, (v, c) => { TitleLayout.IsVisible = false; });

                bottomGrid.TranslateTo(gridX, 70, 250u, Easing.CubicOut);
                bottomGrid.FadeTo(0, 150);

                previousScrollPosition = e.ScrollY;
            }
            else if (previousScrollPosition > e.ScrollY) //up
            {
                //TitleLayout.Opacity = 1;
                //TitleLayout.TranslateTo(titleX, titleY, 250u, Easing.CubicOut);

                //TitleLayout.IsVisible = true;
                //new Animation
                //{
                //    {
                //        0, 0.5, new Animation(v => TitleLayout.TranslationY = v,TitleLayout.TranslationY,titleY,
                //            Easing.CubicOut)
                //    },
                //    {
                //        0, 1, new Animation(v => TitleLayout.Opacity = v,
                //            0,
                //            1)
                //    }
                //}.Commit(this, "VisiblePicker", 10, 200u);

                bottomGrid.Opacity = 1;
                bottomGrid.TranslateTo(gridX, gridY, 200u, Easing.CubicOut);
            }
            previousScrollPosition = e.ScrollY;
        }
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var mccheyne = e.SelectedItem as MccheyneContent;
            shareRangeString = $"({mccheyne.Book}{mccheyne.Verse}) {mccheyne.Content}";

            _mannaTextClickSheet.textLabel.Text = shareRangeString;

            _bottomSheet.Show();

            ((ListView)sender).SelectedItem = null;
        }

        private async void OnReportTapped(object sender, EventArgs e)
        {
            FirebaseEvent.eventTracker.SendEvent("mccheyn_report");

            var address = "jinyeob07@gmail.com";
            await Clipboard.SetTextAsync(address);
            await DisplayAlert("클립보드에 복사됨", address, "확인");
        }

        private void OnBackgroundTapped(object sender, EventArgs e)
        {
            backgroundBoxView.IsVisible = false;
            datepicker.Unfocus();
        }

        private void OnDatepickerUnfocused(object sender, FocusEventArgs e)
        {
            backgroundBoxView.IsVisible = false;
        }

        private async void OnMemoButtonClicked(object sender, EventArgs e)
        {
            _bottomSheet.Hide();
            _memoPopup.SetBibleText(shareRangeString);
            await PopupNavigation.Instance.PushAsync(_memoPopup);
        }

        private async void OnCoppyButtonClicked(object sender, EventArgs e)
        {
            FirebaseEvent.eventTracker.SendEvent("mccheyn_text_coppy");

            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", null, "확인");
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
            FirebaseEvent.eventTracker.SendEvent("mccheyn_text_share");

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

        private async void OnSaveButtonClicked(object sender, string memoText)
        {
            var memoItem = new MemoItem
            {
                Date = DateTime.Now,
                Verse = shareRangeString,
                Note = memoText
            };
            await App.Database.SaveItemAsync(memoItem);
        }

    }
}
