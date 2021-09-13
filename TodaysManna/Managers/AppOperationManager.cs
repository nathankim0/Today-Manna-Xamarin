using System;
using System.Diagnostics;
using TodaysManna.Services;
using TodaysManna.ViewModel;

namespace TodaysManna.Managers
{
    public static class AppOperationManager
    {
        public static async void CreateData()
        {
            try
            {
                GetJsonService.InitMcchyneData();
            }
            catch (Exception e)
            {
                Debug.Fail("InitMcchyneData\n" + e.Message);
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("맥체인 불러오기 오류", "", "확인");
            }
            try
            {
                _ = await GetMannaService.InitMannaData();
            }
            catch (Exception e)
            {
                Debug.Fail("InitMannaData\n" + e.Message);
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("만나 불러오기 오류", "", "확인");
            }
        }
    }
}
