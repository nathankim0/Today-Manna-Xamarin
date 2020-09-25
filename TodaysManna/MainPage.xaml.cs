using System;
using Xamarin.Forms;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text;
using Plugin.Clipboard;

namespace TodaysManna
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            BindingContext = new MainViewModel(Navigation);

        }
    }
}
