using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TodaysManna
{
    public partial class TabMainPage : TabbedPage
    {
        public TabMainPage()
        {
            //NavigationPage.SetHasBackButton(this, false);

            var mannaPage = new NavigationPage(new MannaPage())
            {
                IconImageSource = "Tab_Manna",
                Title = "만나"
            };

            var mccheynePage = new NavigationPage(new McchainPage())
            {
                IconImageSource = "Tab_Mc",
                Title = "맥체인"
            };


            Children.Add(mannaPage);
            Children.Add(mccheynePage);

        }
    }
}
