using System;
using Xamarin.Forms;

namespace TodaysManna
{
    public static class FirebaseEventService
    {
        public static IEventTracker eventTracker = DependencyService.Get<IEventTracker>();

        public static void SendEventOnPlatformSpecific(string message)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    eventTracker.SendEvent(message);
                    break;
                case Device.macOS:
                case Device.UWP:
                    break;
                default:
                    break;
            }
        }
    }
}
