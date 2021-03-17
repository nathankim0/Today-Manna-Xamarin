using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class NTabViewWithXaml : ContentPage
    {
        public NTabViewWithXaml()
        {
            InitializeComponent();
        }

        void OnFabTabTapped(object sender, TabTappedEventArgs e)
        {
            DisplayAlert("NTabViewWithXaml", "Tab Tapped.", "Ok");
        }
    }
}
