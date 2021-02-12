using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace TodaysManna.Popups
{
    public partial class ErrorPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ErrorPopup(string message)
        {
            InitializeComponent();
            errorLabel.Text = message;
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
