using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using TodaysManna.Views;
using TodaysManna.ViewModel;

namespace TodaysManna
{
    public partial class NTabViewWithXaml : ContentPage
    {
        public NTabViewWithXaml()
        {
            InitializeComponent();
        }

        void OnFabTabTapped(object sender, TabTappedEventArgs e)
        {
            DisplayAlert("NTabViewWithXaml", "Tab Tapped.", "Ok");
        }
    }

    public class NTabView : ContentPage
    {
        public NTabView()
        {
            On<iOS>().SetUseSafeArea(true);
            On<iOS>().SetPrefersHomeIndicatorAutoHidden(true);

            var tabView = new TabView
            {
                TabStripPlacement = TabStripPlacement.Bottom,
                TabStripBackgroundColor = Color.White,
                TabStripHeight = 60,
                IsSwipeEnabled = false,
                IsTabTransitionEnabled = false,
            };

            var tabViewItem0 = new TabViewItem
            {
                Text = "만나",
                
            };
            tabViewItem0.Content = new MannaView();
            tabViewItem0.Icon = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookOpenVariant,
            };
            tabViewItem0.Icon.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            var tabViewItem1 = new TabViewItem
            {
                Text = "맥체인"
            };
            tabViewItem1.Content = new MccheyneView();
            tabViewItem1.Icon = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookOpenPageVariantOutline,
            };
            tabViewItem1.Icon.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            var tabViewItem2 = new TabViewItem
            {
                Text = "체크리스트"
            };

            //todo LazyView Test
            var lazyView = new LazyView<MccheyneCheckListView>();
            lazyView.BindingContext = MccheyneCheckViewModel.Current;
            lazyView.SetBinding(BaseLazyView.IsLoadedProperty, "Loaded");

            //tabViewItem2.Content = App.mccheyneCheckListPage;
            tabViewItem2.Icon = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.CheckBoxMultipleOutline,
            };
            tabViewItem2.Icon.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            var tabViewItem3 = new TabViewItem
            {
                Text = "캘린더"
            };
            //tabViewItem3.Content = new MannaCalendarView();
            tabViewItem3.Icon = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.CalendarOutline,
            };
            tabViewItem3.Icon.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            var tabViewItem4 = new TabViewItem
            {
                Text = "체크리스트"
            };
            tabViewItem4.Content = new MyPageView();
            tabViewItem4.Icon = new FontImageSource
            {
                FontFamily = "materialdesignicons",
                Glyph = FontIcons.BookmarkOutline,
            };
            tabViewItem4.Icon.SetAppThemeColor(FontImageSource.ColorProperty, Color.Black, Color.White);

            tabView.TabItems.Add(tabViewItem0);
            tabView.TabItems.Add(tabViewItem1);
            tabView.TabItems.Add(tabViewItem2);
            //tabView.TabItems.Add(tabViewItem3);
            tabView.TabItems.Add(tabViewItem4);

            Content = new Grid
            {
                Children = { tabView }
            };

            tabView.SelectionChanged += TabView_SelectionChanged;
        }

        private void TabView_SelectionChanged(object sender, TabSelectionChangedEventArgs e)
        {
            switch (e.NewPosition)
            {
                case 0:
                    FirebaseEvent.eventTracker.SendEvent("view_navMannaPage");
                    break;
                case 1:
                    FirebaseEvent.eventTracker.SendEvent("view_navMccheynePage");
                    break;
                case 2:
                    FirebaseEvent.eventTracker.SendEvent("view_navMccheyneCheckListPage");
                    App.mccheyneCheckListPage.ScrollToToday();
                    break;
                //case 3:
                //    break;
                case 4:
                    FirebaseEvent.eventTracker.SendEvent("view_navMyPage");
                    break;
            }
        }
    }
}
