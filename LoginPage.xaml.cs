using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace htmlparsing
{
    public partial class LoginPage : ContentPage
    {
        private string _Id,_Passwd;
        
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            nav();

            entry1.Focus();
        }
        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new infoPage());
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
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

        async public void LoginFunc()
        {
            if (string.IsNullOrWhiteSpace(entry1.Text)|| string.IsNullOrWhiteSpace(entry2.Text))
            {
                await DisplayAlert("Login Failed", "Please check your ID and Password.", "OK");
            }
            else
            {
                _Id = entry1.Text;
                _Passwd = entry2.Text;

                Application.Current.Properties["ID"] = _Id;
                Application.Current.Properties["PASSWD"] = _Passwd;
                Application.Current.SavePropertiesAsync();

                await Navigation.PushAsync(new MainPage(_Id, _Passwd));
            }
        }


        async public void nav()
        {
            if (Application.Current.Properties["ID"] == null || Application.Current.Properties["PASSWD"] == null)
            {
                //await Navigation.PushAsync(new LoginPage());

            }
            else
            {
                if (
                    (
                    Application.Current.Properties.ContainsKey("ID") && Application.Current.Properties.ContainsKey("PASSWD")
                    )
                    && (
                    Application.Current.Properties["ID"].ToString() != "" && Application.Current.Properties["PASSWD"].ToString() != ""
                    )
                    )
                {
                    await Navigation.PushAsync(new MainPage(Application.Current.Properties["ID"].ToString(), Application.Current.Properties["PASSWD"].ToString()));
                }
                else
                {
                    //await Navigation.PushAsync(new LoginPage());
                }
            }
        }

    }
}
