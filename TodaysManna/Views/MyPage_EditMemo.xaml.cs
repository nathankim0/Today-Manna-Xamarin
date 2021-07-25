﻿using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MyPage_EditMemo : ContentPage
    {
        public MyPage_EditMemo()
        {
            InitializeComponent();
        }
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("editmemo_save_buttonclicked");
            //((MemoItem)BindingContext).Date = DateTime.Now;
            await App.Database.SaveItemAsync((MemoItem)BindingContext);
            await Navigation.PopAsync();
        }

        private async void OnShareClicked(object sender, EventArgs e)
        {
            if(!(BindingContext is MemoItem memoItem)) { return; }

            FirebaseEventService.SendEventOnPlatformSpecific("editmemo_share");

            string verse = "";
            string note = "";
            string text = "";

            try
            {
                verse = memoItem.Verse;
                note = memoItem.Note;
                text = $"{verse}\n{note}";
            }
            catch(Exception exception)
            {
                System.Diagnostics.Debug.Fail("OnsharedClicked " + exception.Message);
            }

            try
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = text,
                    Title = "공유"
                });
            }
            catch(Exception exception)
            {
                System.Diagnostics.Debug.Fail("OnsharedClicked ShareTextRequest " + exception.Message);
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            FirebaseEventService.SendEventOnPlatformSpecific("editmemo_delete");

            if (await DisplayAlert("", "정말 삭제하시겠습니까?", "삭제", "취소"))
            {
                await App.Database.DeleteItemAsync((MemoItem)BindingContext);
                await Navigation.PopAsync();
            }
        }

        protected override async void OnDisappearing()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("editmemo_save_disappearing");

            //((MemoItem)BindingContext).Date = DateTime.Now;
            await App.Database.SaveItemAsync((MemoItem)BindingContext);

            base.OnDisappearing();
        }
    }
}
