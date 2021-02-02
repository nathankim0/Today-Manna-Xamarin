using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using Syncfusion.XForms.Buttons;

namespace TodaysManna.Views
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.PageSheet);

            InitializeComponent();
            BindingContext = new MccheyneCheckViewModel();
        }

        private async void MaterialChip_ActionImageTapped(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("dasd", null, "ok");
        }

        void SfChip_Clicked(System.Object sender, System.EventArgs e)
        {
            var t = sender as SfChip;
            t.BackgroundColor = Color.Red;
        }
    }
}