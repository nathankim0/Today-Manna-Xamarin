using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TodaysManna.Models;
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

            myViewModel.deleted += async (s, e) =>
            {
                collectionView.ItemsSource = await App.Database.GetItemsAsync();
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
        public EventHandler deleted;
        public ICommand DeleteCommand => new Command<MemoItem>(RemoveItem);

        private async void RemoveItem(MemoItem memoItem)
        {
            await App.Database.DeleteItemAsync(memoItem);
            deleted?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
