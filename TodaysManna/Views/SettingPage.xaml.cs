using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
using TodaysManna.ViewModel;
using Syncfusion.XForms.Buttons;
using TodaysManna.Models;
using System.Linq;

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
    }
}