﻿using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class ErrorPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ErrorPopup()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        public void SetMessage(string text)
        {
            errorLabel.Text = text;
        }
    }
}
