using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace htmlparsing
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage(entry1.Text, entry2.Text));
       
        }
    }
}
