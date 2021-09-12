using TodaysManna.ViewModelV2;

namespace TodaysManna.ViewsV2
{
    public partial class HomePage : BaseContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel();
        }
    }
}
