using System;
using CoreGraphics;
using UIKit;
using TodaysManna.iOS.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace TodaysManna.iOS.Renderer
{
    public class CustomTabbedPageRenderer : TabbedRenderer
    {
        private MainTabbedPage mainTabbedPage;
        private TabbedPage tabbedPage;

        public CustomTabbedPageRenderer()
        {
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                //mainTabbedPage = (MainTabbedPage)e.NewElement;
                tabbedPage = (TabbedPage)e.NewElement;
            }
            else
            {
                tabbedPage = (TabbedPage)e.OldElement;
            }

            try
            {
                var tabbarController = (UITabBarController)this.ViewController;
                if (null != tabbarController)
                {
                    tabbarController.ViewControllerSelected += OnTabbarControllerItemSelected;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private UIViewController previousTabbedViewController;

        private async void OnTabbarControllerItemSelected(object sender, UITabBarSelectionEventArgs eventArgs)
        {
            //if (previousTabbedViewController != eventArgs.ViewController)
            //{
            //    previousTabbedViewController = eventArgs.ViewController;
            //    return;
            //}
            //previousTabbedViewController = eventArgs.ViewController;

            //if (tabbedPage?.CurrentPage?.Navigation == null) return;

            //if (tabbedPage.CurrentPage.Navigation.NavigationStack.Count > 1)
            //{
            //    Debug.WriteLine("PopToRootAsync");
            //    await tabbedPage.CurrentPage.Navigation.PopToRootAsync();
            //}
            //else if (tabbedPage.CurrentPage.Title != null)
            //{
            //    if (tabbedPage.CurrentPage.Title.Equals("만나"))
            //    {
            //        mainTabbedPage.ScrollMannaToTop();
            //    }
            //    else if (tabbedPage.CurrentPage.Title.Equals("맥체인"))
            //    {
            //        mainTabbedPage.ScrollMccheyneToTop();
            //    }
            //    else if (tabbedPage.CurrentPage.Title.Equals("체크리스트"))
            //    {
            //        mainTabbedPage.ScrollCheckListToTop();
            //    }
            //    else if (tabbedPage.CurrentPage.Title.Equals("메모"))
            //    {
            //        mainTabbedPage.ScrollMemoToTop();
            //    }
            //}
        }

        private void SetBorder()
        {
            // 새로운 보더를 만들자. (색 설정 가능) 
            var view = new UIView(new CGRect(0, 0, TabBar.Frame.Width, 1))
            {
                BackgroundColor = Color.FromRgb(0x00, 0x00, 0x00).ToUIColor(),
                Alpha = (System.nfloat)0.2
            };
            // 새로만든 뷰를 탭바에 추가.
            TabBar.AddSubview(view);
        }
    }
}
