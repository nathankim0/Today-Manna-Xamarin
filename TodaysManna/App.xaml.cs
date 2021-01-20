using System;
using TodaysManna.Views;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class App : Application
    {
        public App()
        {
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
