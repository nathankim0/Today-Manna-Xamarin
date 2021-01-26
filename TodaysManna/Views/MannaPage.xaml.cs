﻿using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using TodaysManna.ViewModel;

namespace TodaysManna.Views
{
    public partial class MannaPage : ContentPage
    {
        readonly MannaViewModel mannaViewModel = new MannaViewModel();
        public MannaPage()
        {
            InitializeComponent();

            BindingContext = mannaViewModel;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnShareLabelTapped;
            copyButton.GestureRecognizers.Add(tapGesture);
            // UIApplication.SharedApplication.ApplicationIconBadgeNumber = -1;
        }
        private async void OnShareLabelTapped(object sender, EventArgs args)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = copyButton.Text,
                Title = "공유"
            });
        }

        private async void OnShareButtonClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = today.Text + "\n\n" + verse.Text + "\n\n" + mannaViewModel.AllString,
                Title = "공유"
            });
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var manna = e.SelectedItem as MannaContent;

            var verseText = verse.Text;
            var tmpRangeString = verseText.Substring(0, verseText.IndexOf(":"));

            var shareRangeString = $"({tmpRangeString}:{manna.Number}){manna.MannaString}";

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = shareRangeString,
                Title = "공유"
            });
            ((ListView)sender).SelectedItem = null;
        }

       
        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new SettingPage());
        }
        private async void OnEnglishButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var uri = new Uri(((MannaViewModel)BindingContext)._completeAppUrl);
                await Browser.OpenAsync(uri, BrowserLaunchMode.External);
            }
            catch
            {
                var uri = new Uri(((MannaViewModel)BindingContext)._completeUrl);
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }   
        }
    }
}
