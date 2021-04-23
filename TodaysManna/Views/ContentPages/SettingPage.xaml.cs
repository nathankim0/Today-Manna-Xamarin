using TodaysManna.ViewModel;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
            BindingContext = new SettingViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as SettingViewModel).GetAuthAndIsDatabase();
        }
    }
}
