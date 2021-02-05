using Foundation;
using UIKit;
using UserNotifications;
using Syncfusion.XForms.iOS.EffectsView;
using Xamarin.Forms.Platform.iOS;

namespace TodaysManna.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Syncfusion.XForms.iOS.Buttons.SfChipRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfChipGroupRenderer.Init();
            SfEffectsViewRenderer.Init();  //Initialize only when effects view is added to Listview.

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        
    }
}
