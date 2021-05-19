using Android.App;
using Android.Content;
using TodaysManna.Controls;
using TodaysManna.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using ZXing.Net.Mobile.Forms.Android;

[assembly: ExportRenderer(typeof(CustomZXingScannerView), typeof(CustomZXingScannerViewRenderer))]
namespace TodaysManna.Droid.Renderer
{
    public class CustomZXingScannerViewRenderer : ZXingScannerViewRenderer
    {

        public CustomZXingScannerViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ZXingScannerView> e)
        {
            base.OnElementChanged(e);

            formsView = Element;

            if (zxingSurface == null)
            {
                // Process requests for autofocus

                //var currentContext = Android.App.Application.Context;

                zxingSurface = new ZXingSurfaceView(Context as Activity, formsView.Options);

                //zxingSurface = new ZXingSurfaceView(currentContext, formsView.Options);

            }
        }
    }
}
