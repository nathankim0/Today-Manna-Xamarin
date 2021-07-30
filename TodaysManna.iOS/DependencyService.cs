using System.Collections.Generic;
using Firebase.Analytics;
using Foundation;
using TodaysManna.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSKeyboardHelper))]
[assembly: Dependency(typeof(EventTrackerIOS))]
[assembly: Dependency(typeof(IClearCookiesImplementation))]
[assembly: Dependency(typeof(StatusBar))]
[assembly: Dependency(typeof(HapticFeedback))]

namespace TodaysManna.iOS
{
    public class iOSKeyboardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }

    public class EventTrackerIOS : IEventTracker
    {
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                { paramName, value }
            });
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                Analytics.LogEvent(eventId, (Dictionary<object, object>)null);
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters)
            {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary =
                NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            Analytics.LogEvent(eventId, parametersDictionary);
        }
    }

    public class IClearCookiesImplementation : IClearCookies
    {
        public void Clear()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);
        }
    }

    public class StatusBar : IStatusBar
    {
        public int GetHeight()
        {
            return (int)UIApplication.SharedApplication.StatusBarFrame.Height;
        }
    }

    public class HapticFeedback : IHapticFeedback
    {
        private UISelectionFeedbackGenerator uiSelection;
        public void Run()
        {
            if (uiSelection == null)
            {
                uiSelection = new UISelectionFeedbackGenerator();
                uiSelection.Prepare();
            }
            uiSelection.SelectionChanged();
        }
    }
}
