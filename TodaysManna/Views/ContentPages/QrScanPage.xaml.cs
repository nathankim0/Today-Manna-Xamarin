using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace TodaysManna
{
    public partial class QrScanPage : ContentPage
    {
        public QrScanPage()
        {
            InitializeComponent();

            var myValue = Preferences.Get("QRresults", "");
            editor.Text += myValue;
        }

        public void Handle_OnScanResult(Result result)
        {
            //string temp = "";
            Device.BeginInvokeOnMainThread(async () =>
            {
                var temp = await DisplayPromptAsync("온도", "");
                //await DisplayAlert("Scanned result", result.Text, "OK");

                editor.Text += result.Text + temp + "/x" + "\n";
            });

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