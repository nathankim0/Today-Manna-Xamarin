using System;
using TodaysManna.Datas;
using TodaysManna.ViewModel;
using TodaysManna.Views;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace TodaysManna
{
    public partial class App : Xamarin.Forms.Application
    {
        static MemoItemDatabase database;
        static MccheyneCheckViewModel mccheyneCheckViewModel;

        public App()
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzkyMTQ5QDMxMzgyZTM0MmUzMEp5TmpEcTZabW5hTFBycEp4S2RpRlo0ckxvOHRZMnR1SW0wOFM2c0lSa009");
            //Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

            InitializeComponent();
            MainPage = new MainTabbedPage();
        }

        public static MemoItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new MemoItemDatabase();
                }
                return database;
            }
        }

        public static MccheyneCheckViewModel McCheckViewModel
        {
            get
            {
                if (mccheyneCheckViewModel == null)
                {
                    mccheyneCheckViewModel = new MccheyneCheckViewModel();
                }
                return mccheyneCheckViewModel;
            }
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
