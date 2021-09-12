using TodaysManna.ViewsV2;
using TodaysManna.Managers;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppOperationManager.CreateData();

            MainPage = new MainTabbedPageV2();
        }
       
        protected override void OnStart()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("start_app");
        }

        protected override void OnSleep()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("sleep_app");
        }
    }
}
