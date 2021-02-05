using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using System.Linq;
using System;

namespace TodaysManna.Views
{
    public partial class MccheyneCheckListPage : ContentPage
    {
        public MccheyneCheckListPage(/*MccheyneCheckViewModel mccheyneCheckViewModel*/)
        {
            //On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.PageSheet);
         
            InitializeComponent();
            BindingContext = new MccheyneCheckViewModel();// mccheyneCheckViewModel;
        }
        void ScrollToToday()
        {
            var todayMccheyne = (BindingContext as MccheyneCheckViewModel).MccheyneCheckList.Where(x => x.Date == DateTime.Now.ToString("M-d")).FirstOrDefault();
            collectionView.ScrollTo(todayMccheyne, null, Xamarin.Forms.ScrollToPosition.Start, true);
        }
        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
        protected override void OnAppearing()
        {
            ScrollToToday();
        }
        protected override void OnBindingContextChanged()
        {
            if(!(BindingContext is MccheyneCheckViewModel)) { return; }
            base.OnBindingContextChanged();
            ScrollToToday();

        }
    }
}