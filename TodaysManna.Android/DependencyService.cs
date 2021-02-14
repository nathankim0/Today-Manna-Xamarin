using System;
using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using TodaysManna.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidKeyboardHelper))]
namespace TodaysManna.Droid
{
    public class DroidKeyboardHelper : IKeyboardHelper
    {
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
}
