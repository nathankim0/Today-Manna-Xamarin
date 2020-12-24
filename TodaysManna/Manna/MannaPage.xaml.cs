using System;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        public MannaPage()
        {
            InitializeComponent();
            BindingContext = new MannaViewModel();

            MessagingCenter.Subscribe<MannaViewModel>(this, "loaded", s => { ShareButton.IsVisible = true; });
            MessagingCenter.Subscribe<MannaViewModel>(this, "unloaded", s => { ShareButton.IsVisible = false; });

        }
        private async void ShareClicked(object sender, EventArgs e)
        {
            await (BindingContext as MannaViewModel).ShareFunc();
        }
    }
}
