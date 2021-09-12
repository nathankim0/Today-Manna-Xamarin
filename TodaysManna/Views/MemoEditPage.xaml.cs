using System;
using TodaysManna.Managers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MemoEditPage : ContentPage
    {
        public MemoEditPage()
        {
            InitializeComponent();
        }
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("editmemo_save_buttonclicked");
            await DatabaseManager.Database.SaveItemAsync((MemoItem)BindingContext);
            await Navigation.PopAsync();
        }

        private async void OnShareClicked(object sender, EventArgs e)
        {
            DependencyService.Get<IHapticFeedback>().Run();
            if (!(BindingContext is MemoItem memoItem)) { return; }

            FirebaseEventService.SendEventOnPlatformSpecific("editmemo_share");

            string text = "";
            try
            {
                string verse = memoItem.Verse;
                string note = memoItem.Note;
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
            DependencyService.Get<IHapticFeedback>().Run();
            FirebaseEventService.SendEventOnPlatformSpecific("editmemo_delete");

            if (await DisplayAlert("", "정말 삭제하시겠습니까?", "삭제", "취소"))
            {
                await DatabaseManager.Database.DeleteItemAsync((MemoItem)BindingContext);
                await Navigation.PopAsync();
            }
        }

        protected override async void OnDisappearing()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("editmemo_save_disappearing");

            await DatabaseManager.Database.SaveItemAsync((MemoItem)BindingContext);

            base.OnDisappearing();
        }
    }
}
