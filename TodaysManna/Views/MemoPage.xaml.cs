using System;
using System.Linq;
using TodaysManna.Constants;
using TodaysManna.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MemoPage : ContentPage
    {
        private MemoPageViewModel myPageViewModel = new MemoPageViewModel();
        public MemoPage()
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

                await Navigation.PushAsync(new MemoEditPage
                {
                    BindingContext = e.CurrentSelection.FirstOrDefault() as MemoItem
                });
            }
        }
    }

    
}
