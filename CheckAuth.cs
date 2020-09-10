using System;

using Xamarin.Forms;

namespace htmlparsing
{
    public class CheckAuth : ContentPage
    {
          public CheckAuth()
        {
            nav();
        }
        async public void nav()
        {
            if (Application.Current.Properties["ID"] == null || Application.Current.Properties["PASSWD"] == null)
            {
                await Navigation.PushAsync(new LoginPage());

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
                    await Navigation.PushAsync(new LoginPage());
                }
            }
        }
    }
}

