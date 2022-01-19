using TodaysManna.Managers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppOperationManager.CreateData();

            MainPage = new MainTabbedPage();

            VersionTracking.Track();
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
