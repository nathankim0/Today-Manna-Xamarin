using Android.Content;
using Android.Views;
using Google.Android.Material.Navigation;

using TodaysManna.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace TodaysManna.Droid.Renderer
{
    public class CustomTabbedPageRenderer : TabbedPageRenderer, NavigationBarView.IOnItemSelectedListener
    {
        private MainTabbedPage tabbedPage;

        public CustomTabbedPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                tabbedPage = (MainTabbedPage)e.NewElement;
            }
            else
            {
                tabbedPage = (MainTabbedPage)e.OldElement;
            }
        }

        IMenuItem previousTabbedItem;
        bool isFirst = false;

        bool NavigationBarView.IOnItemSelectedListener.OnNavigationItemSelected(IMenuItem item)
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
                if ((tabbedPage.CurrentPage.Navigation.NavigationStack?.Count ??0) > 1)
                {
                    isFirst = true;
                    PopToRoot();
                }
                else if (tabbedPage.CurrentPage.Title != null)
                {
                    if (tabbedPage.CurrentPage.Title.Equals(TitleNames.Manna))
                    {
                        isFirst = true;
                        tabbedPage.ScrollMannaToTop();
                    }
                    else if (tabbedPage.CurrentPage.Title.Equals(TitleNames.Mccheyne))
                    {
                        isFirst = true;
                        tabbedPage.ScrollMccheyneToTop();
                    }
                    else if (tabbedPage.CurrentPage.Title.Equals(TitleNames.CheckList))
                    {
                        isFirst = true;
                        tabbedPage.ScrollCheckListToTop();
                    }
                    else if (tabbedPage.CurrentPage.Title.Equals(TitleNames.Memo))
                    {
                        isFirst = true;
                        tabbedPage.ScrollMemoToTop();
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
