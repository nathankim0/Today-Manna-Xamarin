using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TodaysManna.Controls.Popups
{
    public partial class SelectionChangedPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public EventHandler memobuttonClicked;
        public EventHandler copybuttonClicked;
        public EventHandler sharebuttonClicked;
        public EventHandler cancelbuttonClicked;

        public SelectionChangedPopup()
        {
            InitializeComponent();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return false;
        }

        private void OnMemoButtonClicked(object sender, EventArgs e)
        {
            memobuttonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnCopyButtonClicked(object sender, EventArgs e)
        {
            copybuttonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnShareButtonClicked(object sender, EventArgs e)
        {
            sharebuttonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            cancelbuttonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
