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

        private readonly MannaPage mannaPage = new MannaPage();
        private readonly MccheynePage mccheynePage = new MccheynePage();
        private readonly MccheyneCheckListPage mccheyneCheckListPage = new MccheyneCheckListPage();
        private readonly MemoPage memoPage = new MemoPage();
        private readonly SettingPage settingPage = new SettingPage();

        public MainTabbedPage()
        {
            On<iOS>().SetUseSafeArea(true);
            On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);

            On<Android>().DisableSmoothScroll();
            On<Android>().DisableSwipePaging();
            On<Android>().SetOffscreenPageLimit(4);
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            navMannaPage = new NavigationPage(mannaPage)
            {
                Title = TitleNames.Manna,
                IconImageSource = new FontImageSource
                {
                    FontFamily = "materialdesignicons",
                    Glyph = FontIcons.HomeOutline,
                }
            };

            navMccheynePage = new NavigationPage(mccheynePage)
            {
                Title = TitleNames.Mccheyne,
                IconImageSource = new FontImageSource
                {
                    FontFamily = "materialdesignicons",
                    Glyph = FontIcons.BookOpenPageVariantOutline,
                }
            };

            navMccheyneCheckListPage = new NavigationPage(mccheyneCheckListPage)
            {
                Title = TitleNames.CheckList,
                IconImageSource = new FontImageSource
                {
                    FontFamily = "materialdesignicons",
                    Glyph = FontIcons.CheckBoxMultipleOutline,
                }
            };

            navMemoPage = new NavigationPage(memoPage)
            {
                Title = TitleNames.Memo,
                IconImageSource = new FontImageSource
                {
                    FontFamily = "materialdesignicons",
                    Glyph = FontIcons.BookmarkOutline,
                }
            };

            navSettingPage = new NavigationPage(settingPage)
            {
                Title = TitleNames.Settings,
                IconImageSource = new FontImageSource
                {
                    FontFamily = "materialdesignicons",
                    Glyph = FontIcons.ReorderHorizontal,
                }
            };

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