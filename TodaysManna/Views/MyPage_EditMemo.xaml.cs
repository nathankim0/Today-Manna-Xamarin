using System;
using System.Collections.Generic;
using TodaysManna.Models;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class MyPage_EditMemo : ContentPage
    {
        public MyPage_EditMemo()
        {
            InitializeComponent();
        }
        async void OnSaveClicked(object sender, EventArgs e)
        {
            await App.Database.SaveItemAsync((MemoItem)BindingContext);
            await Navigation.PopAsync();
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            await App.Database.DeleteItemAsync((MemoItem)BindingContext);
            await Navigation.PopAsync();
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
