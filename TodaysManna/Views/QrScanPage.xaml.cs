using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;
using System.Reflection;

namespace TodaysManna
{
    public partial class QrScanPage : ContentPage
    {
        Plugin.SimpleAudioPlayer.ISimpleAudioPlayer Player;
        QrScanListPage qrScanListPage = new QrScanListPage();

        public QrScanPage()
        {
            InitializeComponent();

            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.beepsound.mp3");
            Player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            Player.Load(stream);
        }

        public void Handle_OnScanResult(Result result)
        {
            Player.Play();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var temp = await DisplayPromptAsync("온도", "");
                if (temp is null) { return; }

                var texts = Preferences.Get("QRresults", "");
                texts += result.Text + temp + "/x" + "\n";
                Preferences.Set("QRresults", texts);

                qrScanListPage.SetEditor();
            });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(qrScanListPage);
        }
    }
}