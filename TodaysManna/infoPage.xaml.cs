using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Clipboard;

namespace TodaysManna
{
    public partial class infoPage : ContentPage
    {
        public ICommand TapCommand => new Command<string>(async (url) =>
        {
            CrossClipboard.Current.SetText(url);
            await DisplayAlert(url, "메일 주소가 클립보드에 복사되었습니다.", "확인");
        });

        public infoPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        async public void BackToLogin(object sender, EventArgs e)
        {
            Application.Current.Properties["ID"] = "";
            Application.Current.Properties["PASSWD"] = "";
            Application.Current.Properties["ISLOGINED"] = false;

            await Application.Current.SavePropertiesAsync();
            await Navigation.PushAsync(new LoginPage());
        }

    }
}
