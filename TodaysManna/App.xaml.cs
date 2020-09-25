using System;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (Current.Properties.ContainsKey("ISLOGINED"))
            {
                if (!(bool)Current.Properties["ISLOGINED"])
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    MainPage=new NavigationPage(new MainPage());

                }
            }

            else
            {
                if (!Current.Properties.ContainsKey("ID") || !Current.Properties.ContainsKey("PASSWD"))
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    MainPage = new NavigationPage(new MainPage());
                }
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
