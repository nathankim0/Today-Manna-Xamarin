using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MyPage : ContentPage
    {
        private MyPageViewModel myPageViewModel = new MyPageViewModel();
        public MyPage()
        {
            InitializeComponent();
            BindingContext = myPageViewModel;

            myPageViewModel.deleted += OnSwipeViewDeleteClicked;
            myPageViewModel.shared += OnSwipeViewSharedClicked;

            MessagingCenter.Subscribe<MainTabbedPage>(this, MessagingCenterMessage.ScrollMemoToTop, (sender) =>
            {
                collectionView.ScrollTo(0);
            });
        }

        private async void OnSwipeViewDeleteClicked(object sender, MemoItem memoItem)
        {
            if (await DisplayAlert("", "정말 삭제하시겠습니까?", "삭제", "취소"))
            {
                await App.Database.DeleteItemAsync(memoItem);
                collectionView.ItemsSource = await App.Database.GetItemsAsync();
            }
        }

        private async void OnSwipeViewSharedClicked(object sender, MemoItem memoItem)
        {
            try
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = memoItem.Verse + "\n" + memoItem.Note,
                    Title = "공유"
                });
            }
            catch(Exception error)
            {
                System.Diagnostics.Debug.WriteLine(error.Message);
                await Clipboard.SetTextAsync(memoItem.Verse + "\n" + memoItem.Note);
                await DisplayAlert("클립보드에 복사됨", memoItem.Verse + "\n" + memoItem.Note, "확인");
            }

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var memoItems = await App.Database.GetItemsAsync();
            memoItems = memoItems.OrderByDescending(x => x.Date).ToList();

            collectionView.ItemsSource = memoItems;
        }

        private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                FirebaseEventService.SendEventOnPlatformSpecific("mypage_memo_select");

                await Navigation.PushAsync(new MyPage_EditMemo
                {
                    BindingContext = e.CurrentSelection.FirstOrDefault() as MemoItem
                });
            }
        }
    }

    public class MyPageViewModel : INotifyPropertyChanged
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
