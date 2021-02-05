using System;
using TodaysManna.Views;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzkyMTQ5QDMxMzgyZTM0MmUzMEp5TmpEcTZabW5hTFBycEp4S2RpRlo0ckxvOHRZMnR1SW0wOFM2c0lSa009");

            InitializeComponent();
            MainPage = new MainTabbedPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
