using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;
using TabbedPage = Xamarin.Forms.TabbedPage;
using TodaysManna.Views;
using TodaysManna.Constants;

namespace TodaysManna
{
    public partial class MainTabbedPage : TabbedPage
    {
        private readonly NavigationPage navMannaPage;
        private readonly NavigationPage navMccheynePage;
        private readonly NavigationPage navMccheyneCheckListPage;
        private readonly NavigationPage navMemoPage;
        private readonly NavigationPage navSettingPage;

        private readonly MannaPage mannaPage;
        private readonly MccheynePage mccheynePage;
        private readonly MccheyneCheckListPage mccheyneCheckListPage;
        private readonly MemoPage memoPage;
        private readonly SettingPage settingPage;

        public MainTabbedPage()
        {
            On<iOS>().SetUseSafeArea(true);
            On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);

            On<Android>().DisableSmoothScroll();
            On<Android>().DisableSwipePaging();
            On<Android>().SetOffscreenPageLimit(4);
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            mannaPage = new MannaPage();
            navMannaPage = new NavigationPage(mannaPage)
            {
                Title = TitleNames.Manna,
            };
            navMannaPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookOpenVariant,
            };
            navMannaPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            mccheynePage = new MccheynePage();
            navMccheynePage = new NavigationPage(mccheynePage)
            {
                Title = TitleNames.Mccheyne,
            };
            navMccheynePage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookOpenPageVariantOutline,
            };
            navMccheynePage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            mccheyneCheckListPage = new MccheyneCheckListPage();
            navMccheyneCheckListPage = new NavigationPage(mccheyneCheckListPage)
            {
                Title = TitleNames.CheckList,
            };
            navMccheyneCheckListPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.CheckBoxMultipleOutline,
            };
            navMccheyneCheckListPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            memoPage = new MemoPage();
            navMemoPage = new NavigationPage(memoPage)
            {
                Title = TitleNames.Memo,
            };
            navMemoPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookmarkOutline,
            };
            navMemoPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            settingPage = new SettingPage();
            navSettingPage = new NavigationPage(settingPage)
            {
                Title = TitleNames.Settings,
            };
            navSettingPage.IconImageSource = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.ReorderHorizontal,
            };
            navSettingPage.IconImageSource.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            Children.Add(navMannaPage);
            Children.Add(navMccheynePage);
            Children.Add(navMccheyneCheckListPage);
            Children.Add(navMemoPage);
            Children.Add(navSettingPage);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if (CurrentPage.Equals(navMannaPage))
            {
                //if (!Values.IsDeviceIOS && Values.MannaPageLaunchCount <= 1)
                //{
                //    MannaPage.CustomOnAppearing();
                //    Values.MannaPageLaunchCount = 2;
                //}

                FirebaseEventService.SendEventOnPlatformSpecific("view_navMannaPage");
            }
            else if (CurrentPage.Equals(navMccheynePage))
            {
                FirebaseEventService.SendEventOnPlatformSpecific("view_navMccheynePage");
            }
            else if (CurrentPage.Equals(navMccheyneCheckListPage))
            {
                if (!Values.IsDeviceIOS && Values.CheckListPageLaunchCount <= 1)
                {
                    mccheyneCheckListPage.CustomOnAppearing();
                    Values.MannaPageLaunchCount = 2;
                }

                FirebaseEventService.SendEventOnPlatformSpecific("view_navMccheyneCheckListPage");
            }
            else if (CurrentPage.Equals(navMemoPage))
            {
                if (!Values.IsDeviceIOS && Values.MemoPageLaunchCount <= 1)
                {
                    memoPage.CustomOnAppearing();
                    Values.MemoPageLaunchCount = 2;
                }

                FirebaseEventService.SendEventOnPlatformSpecific("view_navMyPage");
            }
            else if (CurrentPage.Equals(navSettingPage))
            {
                if (!Values.IsDeviceIOS && Values.SettingsPageLaunchCount <= 1)
                {
                    settingPage.CustomOnAppearing();
                    Values.SettingsPageLaunchCount = 2;
                }

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