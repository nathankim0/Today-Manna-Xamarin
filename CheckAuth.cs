using System;

using Xamarin.Forms;

namespace htmlparsing
{
    public class CheckAuth : ContentPage
    {
        public CheckAuth()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

