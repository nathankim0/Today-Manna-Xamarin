using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace TodaysManna
{
    public partial class McchainPage : ContentPage
    {

        public McchainPage()
        {
            Title = "맥체인";
            IconImageSource = "Tab_Mc";

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);  // Hide nav bar
            /*
            var header = new Label
            {
                Text = "WebView",
                FontSize = 50,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center

            };
            */
            
            WebView webView = new WebView
            {
                Source = new UrlWebViewSource
                {
                    Url = "http://bible4u.pe.kr/zbxe/?mid=open_read&t_size=20&frameborder=0&scrolling=no&width=750&t_color=000000",
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            

            StackLayout stackLayout = new StackLayout
            {
                Children = {/* header, */webView }

            };
            Content = stackLayout;
            /*
            RefreshView refreshView = new RefreshView();
            ICommand refreshCommand = new Command(() =>
            {
                // IsRefreshing is true
                // Refresh data here
                webView.Source = (webView.Source as UrlWebViewSource).Url;

                refreshView.IsRefreshing = false;
            });
            refreshView.Command = refreshCommand;

            ScrollView scrollView = new ScrollView { Content = stackLayout };

            refreshView.Content = scrollView;
            */
        }

    }
}
