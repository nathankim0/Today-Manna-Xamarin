using System;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MannaTextClickSheet : ContentView
    {
        public EventHandler coppybuttonClicked;
        public EventHandler sharebuttonClicked;
        public EventHandler savebuttonClicked;
        public EventHandler cancelbuttonClicked;

        public MannaTextClickSheet()
        {
            InitializeComponent();
        }

        private void OnCoppyButtonClicked(object sender, EventArgs e)
        {
            coppybuttonClicked?.Invoke(this, EventArgs.Empty);
        }
        private void OnShareButtonClicked(object sender, EventArgs e)
        {
            sharebuttonClicked?.Invoke(this, EventArgs.Empty);
        }
        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            savebuttonClicked?.Invoke(this, EventArgs.Empty);
        }
        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            cancelbuttonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
