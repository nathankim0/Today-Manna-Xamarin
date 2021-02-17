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
        private static MemoItemDatabase database;

        public static MccheyneCheckViewModel mccheyneCheckViewModel;
        public static MccheyneCheckListPage mccheyneCheckListPage;

        public App()
        {
            InitializeComponent();

            mccheyneCheckViewModel = new MccheyneCheckViewModel();
            mccheyneCheckListPage = new MccheyneCheckListPage();

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
