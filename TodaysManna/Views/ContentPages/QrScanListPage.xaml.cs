using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TodaysManna
{
    public partial class QrScanListPage : ContentPage
    {
        public QrScanListPage()
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetEditor();
        }

        public void SetEditor()
        {
            editor.Text = Preferences.Get("QRresults", ""); ;
        }

        private void editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Preferences.Set("QRresults", editor.Text);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(editor.Text);
            await DisplayAlert("복사되었습니다.", "", "확인");
        }
    }
}
