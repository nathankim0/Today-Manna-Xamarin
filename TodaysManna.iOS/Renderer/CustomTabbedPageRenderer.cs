using System;
using CoreGraphics;
using UIKit;
using TodaysManna.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;

using TodaysManna.Views;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CustomTabbedPageRenderer))]
namespace TodaysManna.iOS.Renderers
{
    public class CustomTabbedPageRenderer : TabbedRenderer
    {
        private MainTabbedPage mainTabbedPage;
        readonly nfloat imageYOffset = 3.0f;
        private UIViewController previousTabbedViewController;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            Constants.TabHeight = (int)TabBar.Frame.Height;

            SetTabbarUI();
        }

        public override void ViewSafeAreaInsetsDidChange()
        {
            base.ViewSafeAreaInsetsDidChange();
            Constants.BottomSafeAreaHeight = NativeView.SafeAreaInsets.Bottom;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                mainTabbedPage = (MainTabbedPage)e.NewElement;
                SetTabbarColor();

                //SetBorder();
            }
            else
            {
                mainTabbedPage = (MainTabbedPage)e.OldElement;
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

        private async void OnTabbarControllerItemSelected(object sender, UITabBarSelectionEventArgs eventArgs)
        {
            if (previousTabbedViewController != eventArgs.ViewController)
            {
                previousTabbedViewController = eventArgs.ViewController;
                return;
            }
            previousTabbedViewController = eventArgs.ViewController;

            if (mainTabbedPage?.CurrentPage?.Navigation == null) return;
            if (App.Current.MainPage?.Navigation == null) return;

            if (IsNumberOfPageDepthMoreThanOne())
            {
                Debug.WriteLine("Pop Page");

                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
            else if (mainTabbedPage.CurrentPage.Title != null)
            {
                ScrollToTopOfPage();
            }
        }

        private void ScrollToTopOfPage()
        {
            if (mainTabbedPage.CurrentPage.Title.Equals(TitleNames.Manna))
            {
                mainTabbedPage.ScrollMannaToTop();
            }
            else if (mainTabbedPage.CurrentPage.Title.Equals(TitleNames.Mccheyne))
            {
                mainTabbedPage.ScrollMccheyneToTop();
            }
            else if (mainTabbedPage.CurrentPage.Title.Equals(TitleNames.Memo))
            {
                mainTabbedPage.ScrollMemoToTop();
            }
            else if (mainTabbedPage.CurrentPage.Title.Equals(TitleNames.CheckList))
            {
                mainTabbedPage.ScrollCheckListToTop();
            }
        }

        private static bool IsNumberOfPageDepthMoreThanOne() => Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count > 1;

        private void SetTabbarUI()
        {
            if (TabBar.Items != null)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    //ChangeTabbarSelectedIcon(i);
                    DeleteTabbarTitleText(i);
                }
            }
        }

        private void ChangeTabbarSelectedIcon(int i)
        {
            var iconName = "tabicon_selected" + (i + 1);
            TabBar.Items[i].SelectedImage = UIImage.FromBundle(iconName);
        }

        private void DeleteTabbarTitleText(int i)
        {
            TabBar.Items[i].Title = null;
            TabBar.Items[i].ImageInsets = new UIEdgeInsets(imageYOffset, -2, -imageYOffset, -2);
        }

        private static void SetTabbarColor()
        {
            //UITabBar.Appearance.BackgroundColor = Color.White.ToUIColor();
            //UITabBar.Appearance.BackgroundImage = new UIImage();
            //UITabBar.Appearance.ShadowImage = new UIImage();
            UITabBar.Appearance.SelectedImageTintColor = UIColor.FromRGB(253,185,81);
            //UITabBar.Appearance.TintColor = UIColor.Black.ColorWithAlpha(0.2f);
        }

        private void SetBorder()
        {
            // 새로운 보더를 만들자. (색 설정 가능) 
            var view = new UIView(new CGRect(0, 0, TabBar.Frame.Width, 1))
            {
                BackgroundColor = Color.Transparent.ToUIColor(),
            };
            // 새로만든 뷰를 탭바에 추가.
            TabBar.AddSubview(view);
        }
    }
}
