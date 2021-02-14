using System;
using TodaysManna.Datas;
using TodaysManna.Views;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class App : Xamarin.Forms.Application
    {
        static MemoItemDatabase database;

        public App()
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzkyMTQ5QDMxMzgyZTM0MmUzMEp5TmpEcTZabW5hTFBycEp4S2RpRlo0ckxvOHRZMnR1SW0wOFM2c0lSa009");

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
