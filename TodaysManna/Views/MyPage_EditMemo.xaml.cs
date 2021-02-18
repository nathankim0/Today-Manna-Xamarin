using System;
using TodaysManna.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class MyPage_EditMemo : ContentPage
    {
        public MyPage_EditMemo()
        {
            InitializeComponent();
        }
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            await App.Database.SaveItemAsync((MemoItem)BindingContext);
            await Navigation.PopAsync();
        }

        private async void OnShareClicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = $"{((MemoItem)BindingContext).Verse}\n{((MemoItem)BindingContext).Note}",
                Title = "공유"
            });
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("", "정말 삭제하시겠습니까?", "삭제", "취소"))
            {
                await App.Database.DeleteItemAsync((MemoItem)BindingContext);
                await Navigation.PopAsync();
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
