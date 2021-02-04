using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using Syncfusion.XForms.Buttons;
using TodaysManna.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TodaysManna.Views
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage(/*MccheyneCheckViewModel mccheyneCheckViewModel*/)
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
        void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            //Debug.WriteLine("HorizontalDelta: " + e.HorizontalDelta);
            //Debug.WriteLine("VerticalDelta: " + e.VerticalDelta);
            //Debug.WriteLine("HorizontalOffset: " + e.HorizontalOffset);
            //Debug.WriteLine("VerticalOffset: " + e.VerticalOffset);
            //Debug.WriteLine("FirstVisibleItemIndex: " + e.FirstVisibleItemIndex);
            //Debug.WriteLine("CenterItemIndex: " + e.CenterItemIndex);
            //Debug.WriteLine("LastVisibleItemIndex: " + e.LastVisibleItemIndex);
        }
        protected override void OnBindingContextChanged()
        {
            if(!(BindingContext is MccheyneCheckViewModel)) { return; }
            base.OnBindingContextChanged();
            ScrollToToday();

        }
    }
}