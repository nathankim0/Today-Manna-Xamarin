using System;
using System.Collections.Generic;
using TodaysManna.ViewModel;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.Internals;
using System.Diagnostics;

namespace TodaysManna.Views
{
    public partial class MccheynePage : ContentPage
    {
        private double rightY;
        private double rightX;

        private double leftY;
        private double leftX;

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
            

            //scrollView.ScrollToAsync(0, 0, false);
            if (flag == 1)
            {

            }
            else if (flag == 2)
            {
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents1");
                flag = 1;
                leftImageButton.IsVisible = false;
                rightImageButton.IsVisible = true;
            }
            else if (flag == 3)
            {
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
                flag = 2;
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = true;
            }
            else if (flag == 4)
            {
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
                flag = 3;
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = true;
            }
            mccheyneView.ScrollTo(0, 0, false);
        }
        private void PageToRight()
        {
            //mccheyneView.ScrollTo(0, 0, false);
                //ScrollToAsync(0, 0, false);
            if (flag == 1)
            {
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
                flag = 2;
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = true;
            }
            else if (flag == 2)
            {
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = true;
                flag = 3;
            }
            else if (flag == 3)
            {
                mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents4");
                flag = 4;
                leftImageButton.IsVisible = true;
                rightImageButton.IsVisible = false;
            }
            else if (flag == 4)
            {

            }
            mccheyneView.ScrollTo(0, 0, false);
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

        void Button_Clicked(object sender, EventArgs e)
        {
            datepicker.Date = DateTime.Now;
            (BindingContext as MccheyneViewModel).GetMccheyne();
        }

        void ToolbarItem_Clicked(object sender, System.EventArgs e)
        {
            datepicker.Focus();
        }

        private double previousScrollPosition = 0;
        void mccheyneView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (previousScrollPosition < e.ScrollY)
            {
                //scrolled down
                Debug.WriteLine("down");

                leftImageButton.TranslateTo(leftX, 50, 250, Easing.CubicOut);
                rightImageButton.TranslateTo(rightX, 50, 250, Easing.CubicOut);

                //leftImageButton.FadeTo(0, 200);
                //rightImageButton.FadeTo(0, 200);

                previousScrollPosition = e.ScrollY;
            }
            else
            {
                Debug.WriteLine("up");

                leftImageButton.TranslateTo(leftX, leftY, 200, Easing.CubicOut);
                rightImageButton.TranslateTo(rightX, rightY, 200, Easing.CubicOut);
                //scrolled up
                //leftImageButton.FadeTo(1, 200);
                //rightImageButton.FadeTo(1, 200);

                if (Convert.ToInt16(e.ScrollY) == 0)
                    previousScrollPosition = 0;

            }

            Debug.WriteLine("e.ScrollY: " + e.ScrollY);
            //Debug.WriteLine("VerticalDelta: " + e.VerticalDelta);
            //Debug.WriteLine("HorizontalOffset: " + e.HorizontalOffset);
            //Debug.WriteLine("VerticalOffset: " + e.VerticalOffset);
            //Debug.WriteLine("FirstVisibleItemIndex: " + e.FirstVisibleItemIndex);
            //Debug.WriteLine("CenterItemIndex: " + e.CenterItemIndex);
            //Debug.WriteLine("LastVisibleItemIndex: " + e.LastVisibleItemIndex);
        }
    }
}
