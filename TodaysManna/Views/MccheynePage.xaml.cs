using System;
using System.Collections.Generic;
using TodaysManna.ViewModel;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.Internals;

namespace TodaysManna.Views
{
    public partial class MccheynePage : ContentPage
    {
        public MccheynePage()
        {
            InitializeComponent();
            BindingContext = new MccheyneViewModel();

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
            scrollView.ScrollToAsync(0, 0, false);
            if (flag == 1)
            {

            }
            else if (flag == 2)
            {
                mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents1");
                flag = 1;
               // leftImageButton.IsVisible = false;
            }
            else if (flag == 3)
            {
                mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents2");
                flag = 2;
            }
            else if (flag == 4)
            {
                mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents3");
                flag = 3;
              //  rightImageButton.IsVisible = true;
            }
        }
        private void PageToRight()
        {
            scrollView.ScrollToAsync(0, 0, false);
            if (flag == 1)
            {
                mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents2");
                flag = 2;
               // leftImageButton.IsVisible = true;
            }
            else if (flag == 2)
            {
                mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents3");
                flag = 3;
            }
            else if (flag == 3)
            {
                mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents4");
                flag = 4;
             //   rightImageButton.IsVisible = false;
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
           mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents1");
            flag = 1;
        }

        void Button_Clicked_2(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents2");
            flag = 2;
        }

        void Button_Clicked_3(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents3");
            flag = 3;
        }

        void Button_Clicked_4(object sender, EventArgs e)
        {
            mccheyneView.SetBinding(BindableLayout.ItemsSourceProperty, "MccheyneContents4");
            flag = 4;
        }

        void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            (BindingContext as MccheyneViewModel).today = e.NewDate.ToString("M_d");
            (BindingContext as MccheyneViewModel).GetMccheyne();
        }

        void Button_Clicked(object sender, EventArgs e)
        {
            datepicker.Date = DateTime.Now;
            (BindingContext as MccheyneViewModel).GetMccheyne();
        }
    }
}
