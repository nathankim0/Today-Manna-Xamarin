using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TodaysManna
{
    public partial class McchainPage : ContentPage
    {
        public McchainPage()
        {
            Title = "맥체인";
            IconImageSource = "Tab_Mc";
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "mc'chain",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    }
                }
            };

        }
    }
}
