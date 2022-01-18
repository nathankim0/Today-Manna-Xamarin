using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class MemoAddPage : ContentPage
    {
        public EventHandler<string> SaveButtonClicked;

        public MemoAddPage()
        {
            InitializeComponent();
        }

        public void SetBibleText(string text)
        {
            bibleLabel.Text = text;
        }
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("", "정말 취소하시겠습니까?\n작성 중인 메모는 삭제됩니다.", "예", "돌아가기"))
            {
                await Navigation.PopAsync();
            }
        }
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("", "저장하시겠습니까?", "저장", "취소"))
            {
                SaveButtonClicked?.Invoke(this, editor.Text);
                await Navigation.PopAsync();
            }
        }

    }
}
