using System;
using System.Collections.Generic;
using TodaysManna.ViewModel;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.Internals;
using System.Diagnostics;
using TodaysManna.Models;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using ListView = Xamarin.Forms.ListView;

namespace TodaysManna.Views
{
    public partial class MccheynePage : ContentPage
    {
        //private readonly double rightY;
        //private readonly double rightX;

        //private readonly double leftY;
        //private readonly double leftX;

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

            //rightY = rightImageButton.TranslationY;
            //rightX = rightImageButton.TranslationX;

            //leftY = leftImageButton.TranslationY;
            //leftX = leftImageButton.TranslationX;

            todayLabel.Text = DateTime.Now.ToString("M월d일");

            var leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            var rightSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };

            leftSwipeGesture.Swiped += OnLeftSwiped;
            rightSwipeGesture.Swiped += OnRightSwiped;

            mccheyneView.GestureRecognizers.Add(leftSwipeGesture);
            mccheyneView.GestureRecognizers.Add(rightSwipeGesture);
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
            mccheyneView.ScrollTo(mccheyneView.ItemsSource.Cast<object>().FirstOrDefault(),ScrollToPosition.End, false);
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

        //private static DateTime GetCorrectDateLeapYear(DateTime newDate)
        //{
        //    var dateNow = newDate;
        //    DateTime thisDate = dateNow;
        //   // DateTime lastDay = newDate.AddMonths(1).AddDays(0 - newDate.Day);
        //    if (DateTime.IsLeapYear(dateNow.Year) && ((dateNow.Month == 2 && dateNow.Day > 28) || (dateNow.Month > 2)))
        //    {
        //        thisDate = thisDate.AddDays(1);
        //        if (dateNow.Month == 12 && dateNow.Day == 31)
        //        {
        //            thisDate = dateNow;
        //        }
        //    }

        //    return thisDate;
        //}

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as MccheyneViewModel).today = e.NewDate.ToString("M_d");
            DateTime thisDate = MccheyneViewModel.GetCorrectDateLeapYear(e.NewDate);

            (BindingContext as MccheyneViewModel).GetMccheyne(thisDate);
            (BindingContext as MccheyneViewModel).GetMccheyneRange(thisDate);

            todayLabel.Text = e.NewDate.ToString("M월d일");

        }

        private void OnTodayButtonClicked(object sender, EventArgs e)
        {
            datepicker.Date = DateTime.Now;
        //    (BindingContext as MccheyneViewModel).GetMccheyne();
        }

        private void OnDateButtonClicked(object sender, EventArgs e)
        {
            datepicker.Focus();
        }

        private void OnListViewScrolled(object sender, ScrolledEventArgs e)
        {
            //Debug.WriteLine("e.ScrollY: " + e.ScrollY);
            //scrolled down
            if (previousScrollPosition < e.ScrollY)
            {

                //leftImageButton.TranslateTo(leftX, 70, 250, Easing.CubicOut);
                //centerFrame.TranslateTo(leftX, 70, 250, Easing.CubicOut);
                //rightImageButton.TranslateTo(rightX, 70, 250, Easing.CubicOut);


                //leftImageButton.FadeTo(0, 150);
                //centerFrame.FadeTo(0, 150);
                //rightImageButton.FadeTo(0, 150);

                bottomGrid.TranslateTo(gridX, 70, 250, Easing.CubicOut);
                bottomGrid.FadeTo(0, 150);

                previousScrollPosition = e.ScrollY;
            }
            //scrolled up
            else
            {
                //leftImageButton.Opacity = 1;
                //centerFrame.Opacity = 1;
                //rightImageButton.Opacity = 1;

                //leftImageButton.TranslateTo(leftX, leftY, 200, Easing.CubicOut);
                //centerFrame.TranslateTo(leftX, leftY, 200, Easing.CubicOut);
                //rightImageButton.TranslateTo(rightX, rightY, 200, Easing.CubicOut);

                bottomGrid.Opacity = 1;
                bottomGrid.TranslateTo(gridX, gridY, 200, Easing.CubicOut);

                if (Convert.ToInt16(e.ScrollY) == 0)
                    previousScrollPosition = 0;

            }
        }
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var mccheyne = e.SelectedItem as MccheyneContent;
            var shareRangeString = $"({mccheyne.Book}{mccheyne.Verse}) {mccheyne.Content}";

            await Clipboard.SetTextAsync(shareRangeString);
            await DisplayAlert("클립보드에 복사됨", shareRangeString, "확인");

            //await Share.RequestAsync(new ShareTextRequest
            //{
            //    Text = shareRangeString,
            //    Title = "공유"
            //});
            ((ListView)sender).SelectedItem = null;
        }

        private async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var address = "jinyeob07@gmail.com";
            await Clipboard.SetTextAsync(address);
            await DisplayAlert("클립보드에 복사됨", address, "확인");

            // await Browser.OpenAsync(new Uri($"mailto:{address}"), BrowserLaunchMode.External);
        }

        private async void checkButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SettingPage());
        }
    }
}
