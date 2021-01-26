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

            mccheyneView.ItemAppearing += Handle_ItemAppearing;
        }

        private int _lastItemAppearedIdx;
        private int currentFlag = 1;

        private void Handle_ItemAppearing(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            int currentIdx=-10;
            switch (currentFlag)
            {
                case 1:
                    currentIdx = (BindingContext as MccheyneViewModel).MccheyneContents1.IndexOf(e.Item);
                    break;
                case 2:
                    currentIdx = (BindingContext as MccheyneViewModel).MccheyneContents2.IndexOf(e.Item);
                    break;
                case 3:
                    currentIdx = (BindingContext as MccheyneViewModel).MccheyneContents3.IndexOf(e.Item);
                    break;
                case 4:
                    currentIdx = (BindingContext as MccheyneViewModel).MccheyneContents4.IndexOf(e.Item);
                    break;
            }

            if (currentIdx > _lastItemAppearedIdx)
                topGrid.IsVisible = false;
            else
                topGrid.IsVisible = true;

            switch (currentFlag)
            {
                case 1:
                    _lastItemAppearedIdx = (BindingContext as MccheyneViewModel).MccheyneContents1.IndexOf(e.Item);
                    break;
                case 2:
                    _lastItemAppearedIdx = (BindingContext as MccheyneViewModel).MccheyneContents2.IndexOf(e.Item);
                    break;
                case 3:
                    _lastItemAppearedIdx = (BindingContext as MccheyneViewModel).MccheyneContents3.IndexOf(e.Item);
                    break;
                case 4:
                    _lastItemAppearedIdx = (BindingContext as MccheyneViewModel).MccheyneContents4.IndexOf(e.Item);
                    break;
            }

        }

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
           mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents1");
            currentFlag = 1;
        }

        void Button_Clicked_2(System.Object sender, System.EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
            currentFlag = 2;
        }

        void Button_Clicked_3(System.Object sender, System.EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
            currentFlag = 3;
        }

        void Button_Clicked_4(System.Object sender, System.EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents4");
            currentFlag = 4;
        }

        void DatePicker_DateSelected(System.Object sender, Xamarin.Forms.DateChangedEventArgs e)
        {
            (BindingContext as MccheyneViewModel).today = e.NewDate.ToString("M_d");
            (BindingContext as MccheyneViewModel).GetMccheyne();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            datepicker.Date = DateTime.Now;
           // (BindingContext as MccheyneViewModel).today = DateTime.Now.ToString("M_d");
            (BindingContext as MccheyneViewModel).GetMccheyne();
        }
    }
}
