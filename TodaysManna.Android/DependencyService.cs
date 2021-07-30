using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views.InputMethods;
using Android.Webkit;
using Firebase.Analytics;
using TodaysManna.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidKeyboardHelper))]
[assembly: Dependency(typeof(EventTrackerDroid))]
[assembly: Dependency(typeof(IClearCookiesImplementation))]
[assembly: Dependency(typeof(StatusBar))]

namespace TodaysManna.Droid
{
    public class DroidKeyboardHelper : IKeyboardHelper
    {
        [Obsolete]
        public void HideKeyboard()
        {
            var inputMethodManager = Forms.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && Forms.Context is Activity)
            {
                var activity = Forms.Context as Activity;
                var token = activity.CurrentFocus == null ? null : activity.CurrentFocus.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, 0);
            }
        }
    }
    public class EventTrackerDroid : IEventTracker
    {
        [Obsolete]
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, null);
        }

        [Obsolete]
        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                {paramName, value}
            });
        }

        [Obsolete]
        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            var firebaseAnalytics = FirebaseAnalytics.GetInstance(Forms.Context);

            if (parameters == null)
            {
                firebaseAnalytics.LogEvent(eventId, null);
                return;
            }
             
            var bundle = new Bundle();
            foreach (var param in parameters)
            {
                bundle.PutString(param.Key, param.Value);
            }

            firebaseAnalytics.LogEvent(eventId, bundle);
        }
    }
    public class IClearCookiesImplementation : IClearCookies
    {
        public void Clear()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }

    public class StatusBar : IStatusBar
    {
        public static Activity Activity { get; set; }

        public int GetHeight()
        {

            float statusBarHeight = -1;
            int resourceId = Activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                statusBarHeight = Activity.Resources.GetDimension(resourceId);
            }

            float density = Activity.Resources.DisplayMetrics.Density;

            return (int)(statusBarHeight / density);
        }
    }

    public class HapticFeedback : IHapticFeedback
    {
        public void Run()
        {
            var vibrator = (Vibrator)Android.App.Application.Context.GetSystemService(Context.VibratorService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var effect = VibrationEffect.CreateOneShot(20, VibrationEffect.DefaultAmplitude);
                vibrator?.Vibrate(effect);
            }
            else
            {
#pragma warning disable 618
                vibrator?.Vibrate(20);
#pragma warning restore 618
            }
        }
    }
}
