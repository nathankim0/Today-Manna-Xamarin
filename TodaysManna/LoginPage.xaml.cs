using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class LoginPage : ContentPage
    {
        private string _Id, _Passwd;
        private bool _IsLogined;

        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            entry1.Focus();
        }
        

        async public void LoginFunc()
        {
            if (string.IsNullOrWhiteSpace(entry1.Text) || string.IsNullOrWhiteSpace(entry2.Text))
            {
                await DisplayAlert("로그인 실패!", "아이디와 비밀번호를 확인해주세요.", "확인");
            }
            else
            {
                _Id = entry1.Text; 
                _Passwd = entry2.Text;
                _IsLogined = true;

                Application.Current.Properties["ID"] = _Id;
                Application.Current.Properties["PASSWD"] = _Passwd;
                Application.Current.Properties["ISLOGINED"] = _IsLogined;

                await Application.Current.SavePropertiesAsync();

                await Navigation.PushAsync(new MainPage());
            }
        }

        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            LoginFunc();
        }
        void entry1_Completed(System.Object sender, System.EventArgs e)
        {
            entry2.Focus();
        }
        void entry2_Completed(System.Object sender, System.EventArgs e)
        {
            LoginFunc();
        }
    }
}
