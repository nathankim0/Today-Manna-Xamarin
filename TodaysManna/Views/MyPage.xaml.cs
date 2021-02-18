using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodaysManna.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class MyPage : ContentPage
    {
        private MyViewModel myViewModel = new MyViewModel();
        public MyPage()
        {
            InitializeComponent();
            BindingContext = myViewModel;

            myViewModel.deleted += async (s, memoItem) =>
            {
                if (await DisplayAlert("", "정말 삭제하시겠습니까?", "삭제", "취소"))
                {
                    await App.Database.DeleteItemAsync(memoItem);
                    collectionView.ItemsSource = await App.Database.GetItemsAsync();
                }
            };
            myViewModel.shared += async (s, memoItem) =>
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = memoItem.Verse+"\n"+memoItem.Note,
                    Title = "공유"
                });
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = await App.Database.GetItemsAsync();
        }

        private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new MyPage_EditMemo
                {
                    BindingContext = e.CurrentSelection.FirstOrDefault() as MemoItem
                });
            }
        }
    }
    public class MyViewModel : INotifyPropertyChanged
    {
        public EventHandler<MemoItem> deleted;
        public EventHandler<MemoItem> shared;

        public ICommand DeleteCommand => new Command<MemoItem>(RemoveItem);
        public ICommand ShareCommand => new Command<MemoItem>(ShareItem);

        private void RemoveItem(MemoItem memoItem)
        {
            deleted?.Invoke(this, memoItem);
        }
        private void ShareItem(MemoItem memoItem)
        {
            shared?.Invoke(this, memoItem);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
