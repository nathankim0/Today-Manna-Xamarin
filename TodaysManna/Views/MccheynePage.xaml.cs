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

        private void Handle_ItemAppearing(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            var currentIdx = (BindingContext as MccheyneViewModel).MccheyneContents1.IndexOf(e.Item);

            if (currentIdx > _lastItemAppearedIdx)
                topGrid.IsVisible = false;
            else
                topGrid.IsVisible = true;

            _lastItemAppearedIdx = (BindingContext as MccheyneViewModel).MccheyneContents1.IndexOf(e.Item);
        }

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
           mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents1");
        }

        void Button_Clicked_2(System.Object sender, System.EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents2");
        }

        void Button_Clicked_3(System.Object sender, System.EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents3");
        }

        void Button_Clicked_4(System.Object sender, System.EventArgs e)
        {
            mccheyneView.SetBinding(ListView.ItemsSourceProperty, "MccheyneContents4");
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
