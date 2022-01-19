using System;
using System.Diagnostics;
using System.Linq;
using Rg.Plugins.Popup.Services;
using TodaysManna.Managers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna
{
    public static class AppManager
    {
        private static readonly string EXCEPTION_COMMON_TEXT = "Exception occured at";

        public static async void InitManna()
        {
            var isGetMannaCompleted = await MannaDataManager.GetManna(DateTime.Now);
            if (!isGetMannaCompleted)
            {
                await Application.Current.MainPage.DisplayAlert("만나 불러오기 실패", "새로고침 해주세요", "확인");
            }
        }

        public static void PrintException(string location, string message)
        {
            Debug.WriteLine($"{EXCEPTION_COMMON_TEXT} {location}!\n{message}");
        }

        public static void PrintStartText(string location)
        {
            Debug.WriteLine($"** {location} start! **");
        }

        public static void PrintCompleteText(string location)
        {
            Debug.WriteLine($"** {location} complete! **");
        }

        public static bool IsPopupNavigationNullOrExist()
        {
            return PopupNavigation.Instance.PopupStack?.Any() ?? true;
        }

        public static double GetCurrentTextSize()
        {
            return double.TryParse(Preferences.Get("TextSize", "17"), out var font) ? font : 17;
        }

        public static Color GetCurrentTextColor()
        {
            return Color.FromHex(Preferences.Get("CustomTextColor", Constants.DEFAULT_TEXT_COLOR));
        }

        public static Color GetCurrentBackgroundDimColor()
        {
            return Color.FromHex(Preferences.Get("CustomBackgroundDimColor", Constants.DEFAULT_BACKGROUND_DIM_COLOR));
        }

        public static string GetCurrentLanguageString()
        {
            return Preferences.Get("CurrentLanguage", Language.Korean.ToString());
        }

        public static Language GetCurrentLanguageEnumValue()
        {
            try
            {
                return EnumUtil<Language>.Parse(GetCurrentLanguageString());
            }
            catch
            {
                return Language.Korean;
            }
        }

        public static class EnumUtil<T> { public static T Parse(string s) { return (T)Enum.Parse(typeof(T), s); } }
    }
}
