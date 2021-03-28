using System;
using Xamarin.Forms;

namespace TodaysManna
{
    public static class FirebaseEventService
    {
        public static IEventTracker eventTracker = DependencyService.Get<IEventTracker>();
    }
}
