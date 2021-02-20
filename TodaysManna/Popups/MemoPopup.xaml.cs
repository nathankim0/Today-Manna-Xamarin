﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace TodaysManna.Popups
{
    public partial class MemoPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public EventHandler<string> SaveButtonClicked;
        public MemoPopup()
        {
            InitializeComponent();
        }
        public void SetBibleText(string text)
        {
            bibleLabel.Text = text;
        }
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            SaveButtonClicked?.Invoke(this, editor.Text);
            await PopupNavigation.Instance.PopAsync();
        }


        protected override void OnAppearing()
        {
            editor.Text = "";
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            return false;
            // Return true if you don't want to close this popup page when a back button is pressed
            //return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            return false;
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            //return base.OnBackgroundClicked();
        }

    }
}