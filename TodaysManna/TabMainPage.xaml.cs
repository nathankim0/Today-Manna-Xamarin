using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TodaysManna
{
    public partial class TabMainPage : TabbedPage
    {
        public TabMainPage()
        {
            var navigationPage = new NavigationPage(new MainPage());
            navigationPage.IconImageSource = "Tab_Manna";
            navigationPage.Title = "만나";

            Children.Add(navigationPage);
            Children.Add(new McchainPage());
            Children.Add(new infoPage());

        }
    }
}
