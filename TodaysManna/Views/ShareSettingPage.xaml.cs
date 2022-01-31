using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class ShareSettingPage : ContentPage
    {
        public ShareSettingPage()
        {
            InitializeComponent();

            Padding = new Thickness(0, Constants.StatusBarHeight, 0, 0);

            topTextEditor.Text = AppManager.GetShareTopTextString();
            bottomTextEditor.Text = AppManager.GetShareBottomTextString();
        }

        void topTextEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Preferences.Set("ShareTopText", e.NewTextValue);
        }

        void bottomTextEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Preferences.Set("ShareBottomText", e.NewTextValue);
        }
    }
}
