using System;
using Android.Content;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using TodaysManna.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace TodaysManna.Droid.Renderer
{
    public class CustomTabbedPageRenderer : TabbedPageRenderer, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private TabbedPage tabbedPage;
        private MainTabbedPage mainTabbedPage;

        public CustomTabbedPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                mainTabbedPage = (MainTabbedPage)e.NewElement;
                tabbedPage = (TabbedPage)e.NewElement;
            }
            else
            {
                tabbedPage = (TabbedPage)e.OldElement;
            }
        }

        IMenuItem previousTabbedItem;
        bool isFirst = false;

        bool BottomNavigationView.IOnNavigationItemSelectedListener.OnNavigationItemSelected(IMenuItem item)
        {
            base.OnNavigationItemSelected(item);

            if (previousTabbedItem != item)
            {
                previousTabbedItem = item;
                isFirst = false;
                return true;
            }
            else
            {
                if (!isFirst)
                {
                    isFirst = true;
                    return true;
                }
                else
                {
                    isFirst = false;
                }
            }
            previousTabbedItem = item;


            if (tabbedPage?.CurrentPage?.Navigation == null) { return true; }

            if (Element is MainTabbedPage)
            {
                if (tabbedPage.CurrentPage.Navigation.NavigationStack.Count > 1)
                {
                    Console.WriteLine("PopToRootAsync");
                    isFirst = true;
                    PopToRoot();
                }
                else if (tabbedPage.CurrentPage.Title != null)
                {
                    if (tabbedPage.CurrentPage.Title.Equals("만나"))
                    {
                        isFirst = true;
                        //mainTabbedPage.ScrollMannaToTop();
                    }
                    else if (tabbedPage.CurrentPage.Title.Equals("맥체인"))
                    {
                        isFirst = true;
                        //mainTabbedPage.ScrollMccheyneToTop();
                    }
                    else if (tabbedPage.CurrentPage.Title.Equals("체크리스트"))
                    {
                        isFirst = true;
                        //mainTabbedPage.ScrollCheckListToTop();
                    }
                    else if (tabbedPage.CurrentPage.Title.Equals("메모"))
                    {
                        isFirst = true;
                        //mainTabbedPage.ScrollMemoToTop();
                    }
                }
            }

            return true;
        }

        async void PopToRoot()
        {
            await tabbedPage.CurrentPage.Navigation.PopToRootAsync();
        }
    }
}
