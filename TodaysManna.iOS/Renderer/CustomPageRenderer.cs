using TodaysManna.iOS.Renderers;
using TodaysManna.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MccheyneCheckListPage), typeof(CustomPageRenderer))]
namespace TodaysManna.iOS.Renderers
{
	public class CustomPageRenderer : PageRenderer, IUIGestureRecognizerDelegate
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationController != null)
            {
                NavigationController.NavigationBarHidden = true;
                NavigationController.InteractivePopGestureRecognizer.Delegate = this;
                NavigationController.InteractivePopGestureRecognizer.Enabled = true;
            }
        }
    }
}