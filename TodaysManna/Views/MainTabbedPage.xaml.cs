using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace TodaysManna.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
        NavigationPage navMccheyneCheckListPage;
        public MainTabbedPage()
        {
            //InitializeComponent();

            On<iOS>().SetUseSafeArea(true);
            On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);

            On<Android>().DisableSwipePaging();
            On<Android>().SetOffscreenPageLimit(4);
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            var navMannaPage = new NavigationPage(new MannaPage())
            {
                Title = "만나",
            };
            navMannaPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookOpenVariant,
            };
            navMannaPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            var navMccheynePage = new NavigationPage(new MccheynePage())
            {
                Title = "맥체인",
            };
            navMccheynePage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookOpenPageVariantOutline,
            };
            navMccheynePage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            navMccheyneCheckListPage = new NavigationPage(App.mccheyneCheckListPage)
            {
                Title = "체크리스트",
            };
            navMccheyneCheckListPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.CheckBoxMultipleOutline,
            };
            navMccheyneCheckListPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            var navMyPage = new NavigationPage(new MyPage())
            {
                Title = "메모",
            };
            navMyPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookmarkOutline,
            };
            navMyPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            Children.Add(navMannaPage);
            Children.Add(navMccheynePage);
            Children.Add(navMccheyneCheckListPage);
            Children.Add(navMyPage);


        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if (CurrentPage.Equals(navMccheyneCheckListPage))
            {
                App.mccheyneCheckListPage.ScrollToToday();
            }
        }
    }
}