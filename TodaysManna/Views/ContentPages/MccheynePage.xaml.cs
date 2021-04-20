using System;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;
using ListView = Xamarin.Forms.ListView;
using Rg.Plugins.Popup.Services;
using TodaysManna.ViewModel;
using TodaysManna.Models;

namespace TodaysManna
{
    public partial class MccheynePage : ContentPage
    {
        private readonly BottomSheet _bottomSheet;
        private readonly MannaTextClickSheet _mannaTextClickSheet;
        private readonly MemoPopup _memoPopup;

        private string shareRangeString = "";

        private readonly double gridX;
        private readonly double gridY;

        private int flag = 1;
        private double previousScrollPosition = 0;

        public MccheynePage()
        {
            InitializeComponent();
            BindingContext = new MccheyneViewModel();

            gridX = bottomGrid.TranslationX;
            gridY = bottomGrid.TranslationY;

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

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMccheyneToTop, (sender) =>
            {
                MccheyneViewSctollToTop(true);
            });
        }

        private void PageToLeft()
        {
            try
            {
                // Perform click feedback
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

            switch (flag)
            {
                case 2:
                    centerLocationLabel.Text = "1/4";
                    mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents1");
                    flag = 1;
                    leftImageButton.IsVisible = false;
                    rightImageButton.IsVisible = true;
                    break;
                case 3:
                    centerLocationLabel.Text = "2/4";
                    mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
                    flag = 2;
                    leftImageButton.IsVisible = true;
                    rightImageButton.IsVisible = true;
                    break;
                case 4:
                    centerLocationLabel.Text = "3/4";
                    mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
                    flag = 3;
                    leftImageButton.IsVisible = true;
                    rightImageButton.IsVisible = true;
                    break;
                default:
                    break;
            }
            MccheyneViewSctollToTop(false);
        }

        private void PageToRight()
        {
            try
            {
                // Perform click feedback
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

            switch (flag)
            {
                case 1:
                    centerLocationLabel.Text = "2/4";
                    mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
                    flag = 2;
                    leftImageButton.IsVisible = true;
                    rightImageButton.IsVisible = true;
                    break;
                case 2:
                    centerLocationLabel.Text = "3/4";
                    mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
                    leftImageButton.IsVisible = true;
                    rightImageButton.IsVisible = true;
                    flag = 3;
                    break;
                case 3:
                    centerLocationLabel.Text = "4/4";
                    mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents4");
                    flag = 4;
                    leftImageButton.IsVisible = true;
                    rightImageButton.IsVisible = false;
                    break;
                case 4:
                    break;
                default:
                    break;
            }
            MccheyneViewSctollToTop(false);
        }

        private void MccheyneViewSctollToTop(bool isAnimationEnabled)
        {
            mccheyneView.ScrollTo(mccheyneView.ItemsSource.Cast<object>().FirstOrDefault(), ScrollToPosition.End, isAnimationEnabled);
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
            try
            {
                // Perform click feedback
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_today");

            datepicker.Date = DateTime.Now;
        }

        private void OnDateButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_date");

            backgroundBoxView.IsVisible = true;
            datepicker.Focus();
        }

        private void OnListViewScrolled(object sender, ScrolledEventArgs e)
        {
            if (previousScrollPosition < e.ScrollY) //down
            {
                bottomGrid.TranslateTo(gridX, 70, 250u, Easing.CubicOut);
                bottomGrid.FadeTo(0, 150);

                previousScrollPosition = e.ScrollY;
            }
            else if (previousScrollPosition > e.ScrollY) //up
            {
                bottomGrid.Opacity = 1;
                bottomGrid.TranslateTo(gridX, gridY, 200u, Easing.CubicOut);
            }
            previousScrollPosition = e.ScrollY;
        }
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            try
            {
                // Perform click feedback
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

            var mccheyne = e.SelectedItem as MccheyneContent;
            shareRangeString = $"({mccheyne.Book}{mccheyne.Verse}) {mccheyne.Content}\n";

            _mannaTextClickSheet.textLabel.Text = shareRangeString;

            _bottomSheet.Show();

            ((ListView)sender).SelectedItem = null;
        }

        private async void OnReportTapped(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_report");

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
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_text_coppy");

            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", null, "확인");
        }

        private async void OnTextShareButtonClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("mccheyn_text_share");

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
