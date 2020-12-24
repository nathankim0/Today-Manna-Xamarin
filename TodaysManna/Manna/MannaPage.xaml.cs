using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MannaPage : ContentPage
    {
        public MannaPage()
        {
            InitializeComponent();
            BindingContext = new MannaViewModel();
        }
    }
}
