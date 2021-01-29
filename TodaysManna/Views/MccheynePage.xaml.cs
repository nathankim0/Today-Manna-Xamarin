using System;
using System.Collections.Generic;
using TodaysManna.ViewModel;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.Internals;
using System.Diagnostics;
using TodaysManna.Models;
using Xamarin.Essentials;

namespace TodaysManna.Views
{
    public partial class MccheynePage : ContentPage
    {
        private readonly double rightY;
        private readonly double rightX;

        private readonly double leftY;
        private readonly double leftX;

        public MccheynePage()
        {
            InitializeComponent();
            BindingContext = new MccheyneViewModel();

            rightY = rightImageButton.TranslationY;
            rightX = rightImageButton.TranslationX;

            leftY = leftImageButton.TranslationY;
            leftX = leftImageButton.TranslationX;

            todayLabel.Text = DateTime.Now.ToString("M/dd");

            var leftSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            var rightSwipeGesture = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };

            leftSwipeGesture.Swiped += OnLeftSwiped;
            rightSwipeGesture.Swiped += OnRightSwiped;

            mccheyneView.GestureRecognizers.Add(leftSwipeGesture);
            mccheyneView.GestureRecognizers.Add(rightSwipeGesture);

        }

        int flag = 1;
        private void PageToLeft()
        {
            mccheyneView.ScrollTo(0, 0, false);
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
        }
        private void PageToRight()
        {
            mccheyneView.ScrollTo(0, 0, false);
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

        void Button_Clicked_1(object sender, EventArgs e)
        {
           mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents1");
            flag = 1;
        }

        void Button_Clicked_2(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
            flag = 2;
        }

        void Button_Clicked_3(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
            flag = 3;
        }

        void Button_Clicked_4(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents4");
            flag = 4;
        }

        void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as MccheyneViewModel).today = e.NewDate.ToString("M_d");
            (BindingContext as MccheyneViewModel).GetMccheyne();
            todayLabel.Text = e.NewDate.ToString("M/dd");
        }

        void OnTodayButtonClicked(object sender, EventArgs e)
        {
            datepicker.Date = DateTime.Now;
            (BindingContext as MccheyneViewModel).GetMccheyne();
        }

        void OnDateButtonClicked(object sender, System.EventArgs e)
        {
            datepicker.Focus();
        }

        private double previousScrollPosition = 0;
        void OnListViewScrolled(object sender, ScrolledEventArgs e)
        {
            if (previousScrollPosition < e.ScrollY)
            {
                //scrolled down
                Debug.WriteLine("down");

                leftImageButton.TranslateTo(leftX, 70, 250, Easing.CubicOut);
                centerFrame.TranslateTo(leftX, 70, 250, Easing.CubicOut);
                rightImageButton.TranslateTo(rightX, 70, 250, Easing.CubicOut);

                leftImageButton.FadeTo(0, 150);
                rightImageButton.FadeTo(0, 150);

                previousScrollPosition = e.ScrollY;
            }
            else
            {
                Debug.WriteLine("up");

                leftImageButton.Opacity = 1;
                centerFrame.Opacity = 1;
                rightImageButton.Opacity = 1;

                leftImageButton.TranslateTo(leftX, leftY, 200, Easing.CubicOut);
                centerFrame.TranslateTo(leftX, leftY, 200, Easing.CubicOut);
                rightImageButton.TranslateTo(rightX, rightY, 200, Easing.CubicOut);
                //scrolled up
                if (Convert.ToInt16(e.ScrollY) == 0)
                    previousScrollPosition = 0;

            }

            Debug.WriteLine("e.ScrollY: " + e.ScrollY);
        }
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var manna = e.SelectedItem as MccheyneContent;
            var shareRangeString = $"({manna.Book}) {manna.Content}";

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = shareRangeString,
                Title = "공유"
            });
            ((ListView)sender).SelectedItem = null;
        }
    }
}
