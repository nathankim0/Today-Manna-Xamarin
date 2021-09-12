using Xamarin.Forms;

namespace TodaysManna.ViewsV2
{
    public partial class MainTabbedPageV2 : TabbedPage
    {
        public MainTabbedPageV2()
        {
            InitializeComponent();

            SelectedTabColor = Color.Black;
            BarTextColor = Color.Black;
            BarBackgroundColor = Color.White;

            var navHomePage = new NavigationPage(new HomePage())
            {
                Title = "홈",
            };
            navHomePage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.Home,
            };

            Children.Add(navHomePage);
        }
    }
}
