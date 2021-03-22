using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace TodaysManna.Views
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

        void editor_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            Preferences.Set("QRresults", editor.Text);
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Clipboard.SetTextAsync(editor.Text);
            await DisplayAlert("복사되었습니다.","","확인");
        }
    }
}
