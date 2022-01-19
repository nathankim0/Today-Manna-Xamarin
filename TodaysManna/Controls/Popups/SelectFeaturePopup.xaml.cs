using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TodaysManna.Controls.Popups
{
    public partial class SelectFeaturePopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private static SelectFeaturePopup _instance;
        public static SelectFeaturePopup Instance => _instance ?? (_instance = new SelectFeaturePopup());

        public EventHandler MemobuttonClicked;
        public EventHandler CopybuttonClicked;
        public EventHandler SharebuttonClicked;
        public EventHandler CancelbuttonClicked;

        public SelectFeaturePopup()
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
            MemobuttonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnCopyButtonClicked(object sender, EventArgs e)
        {
            CopybuttonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnShareButtonClicked(object sender, EventArgs e)
        {
            SharebuttonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            CancelbuttonClicked?.Invoke(this, EventArgs.Empty);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
