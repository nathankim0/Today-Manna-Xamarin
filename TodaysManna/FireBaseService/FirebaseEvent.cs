using System;
using Xamarin.Forms;

namespace TodaysManna
{
    public static class FirebaseEvent
    {
        public static IEventTracker eventTracker = DependencyService.Get<IEventTracker>();
    }
}
