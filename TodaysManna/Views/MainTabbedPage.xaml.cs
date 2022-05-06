using Xamarin.Forms;

namespace TodaysManna
{
    public partial class MainTabbedPage : TabbedPage
    {
        private readonly NavigationPage navMannaPage;
        private readonly NavigationPage navMccheynePage;
        private readonly NavigationPage navMccheyneCheckListPage;
        private readonly NavigationPage navMemoPage;

        private readonly MannaPage mannaPage = new MannaPage();
        private readonly MccheynePage mccheynePage = new MccheynePage();
        private readonly MccheyneCheckListPage mccheyneCheckListPage = new MccheyneCheckListPage();
        private readonly MemoPage memoPage = new MemoPage();

        public MainTabbedPage()
        {
            InitializeComponent();

            navMannaPage = new NavigationPage(mannaPage)
            {
                Title = TitleNames.Manna,
                IconImageSource = new FontImageSource
                {
                    FontFamily = "materialdesignicons",
                    Glyph = FontIcons.Home,
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

            Children.Add(navMannaPage);
            Children.Add(navMccheynePage);
            Children.Add(navMccheyneCheckListPage);
            Children.Add(navMemoPage);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if (CurrentPage.Equals(navMannaPage))
            {
                FirebaseEventService.SendEventOnPlatformSpecific("view_navMannaPage");
            }
            else if (CurrentPage.Equals(navMccheynePage))
            {
                FirebaseEventService.SendEventOnPlatformSpecific("view_navMccheynePage");
            }
            else if (CurrentPage.Equals(navMccheyneCheckListPage))
            {
                if (!Constants.IsDeviceIOS && Constants.CheckListPageLaunchCount <= 1)
                {
                    mccheyneCheckListPage.CustomOnAppearing();
                    Constants.MannaPageLaunchCount = 2;
                }

                FirebaseEventService.SendEventOnPlatformSpecific("view_navMccheyneCheckListPage");
            }
            else if (CurrentPage.Equals(navMemoPage))
            {
                if (!Constants.IsDeviceIOS && Constants.MemoPageLaunchCount <= 1)
                {
                    memoPage.CustomOnAppearing();
                    Constants.MemoPageLaunchCount = 2;
                }

                FirebaseEventService.SendEventOnPlatformSpecific("view_navMyPage");
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