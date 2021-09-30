using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;
using TabbedPage = Xamarin.Forms.TabbedPage;
using TodaysManna.Views;
using TodaysManna.Constants;

namespace TodaysManna.ViewsV2
{
    public partial class MainTabbedPageV2 : TabbedPage
    {
        private readonly NavigationPage navHomePage;
        private readonly NavigationPage navMccheynePage;
        private readonly NavigationPage navMccheyneCheckListPage;
        private readonly NavigationPage navMyPage;
        private readonly NavigationPage navSettingPage;

        public MainTabbedPageV2()
        {
            On<iOS>().SetUseSafeArea(true);
            On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);

            On<Android>().DisableSmoothScroll();
            On<Android>().DisableSwipePaging();
            On<Android>().SetOffscreenPageLimit(4);
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            SelectedTabColor = Color.Black;
            BarTextColor = Color.Black;
            BarBackgroundColor = Color.White;

            navHomePage = new NavigationPage(new MannaPage())
            {
                Title = "만나",
            };
            navHomePage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookOpenVariant,
            };
            navHomePage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            navMccheynePage = new NavigationPage(new MccheynePage())
            {
                Title = "맥체인",
            };
            navMccheynePage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookOpenPageVariantOutline,
            };
            navMccheynePage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            navMccheyneCheckListPage = new NavigationPage(new MccheyneCheckListPage())
            {
                Title = "체크리스트",
            };
            navMccheyneCheckListPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.CheckBoxMultipleOutline,
            };
            navMccheyneCheckListPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            navMyPage = new NavigationPage(new MemoPage())
            {
                Title = "메모",
            };
            navMyPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookmarkOutline,
            };
            navMyPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            navSettingPage = new NavigationPage(new SettingPage())
            {
                Title = "설정",
            };
            navSettingPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.ReorderHorizontal,
            };
            navSettingPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            Children.Add(navHomePage);
            Children.Add(navMccheynePage);
            Children.Add(navMccheyneCheckListPage);
            Children.Add(navMyPage);
            Children.Add(navSettingPage);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if (CurrentPage.Equals(navHomePage))
            {
                FirebaseEventService.SendEventOnPlatformSpecific("view_navMannaPage");
            }
            else if (CurrentPage.Equals(navMccheynePage))
            {
                FirebaseEventService.SendEventOnPlatformSpecific("view_navMccheynePage");
            }
            else if (CurrentPage.Equals(navMccheyneCheckListPage))
            {
                FirebaseEventService.SendEventOnPlatformSpecific("view_navMccheyneCheckListPage");
            }
            else if (CurrentPage.Equals(navMyPage))
            {
                FirebaseEventService.SendEventOnPlatformSpecific("view_navMyPage");
            }
            else if (CurrentPage.Equals(navSettingPage))
            {
                FirebaseEventService.SendEventOnPlatformSpecific("view_navSettingPage");
            }
        }

        public void ScrollMannaToTop()
        {
            MessagingCenter.Send(this, MessagingCenterMessage.ScrollMannaToTop);
        }

        public void ScrollMccheyneToTop()
        {
            MessagingCenter.Send(this, MessagingCenterMessage.ScrollMccheyneToTop);
        }

        public void ScrollCheckListToTop()
        {
            MessagingCenter.Send(this, MessagingCenterMessage.ScrollCheckListToTop);
        }

        public void ScrollMemoToTop()
        {
            MessagingCenter.Send(this, MessagingCenterMessage.ScrollMemoToTop);
        }
    }
}