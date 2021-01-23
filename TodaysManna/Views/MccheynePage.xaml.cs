using System;
using System.Collections.Generic;
using TodaysManna.ViewModel;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class MccheynePage : ContentPage
    {
        public MccheynePage()
        {
            InitializeComponent();
            BindingContext = new MccheyneViewModel();
        }
    }
}
