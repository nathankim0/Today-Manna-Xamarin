﻿using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using Xamarin.Forms;
using Plugin.FirebasePushNotification;

namespace TodaysManna.Droid
{
    [Activity(Label = "오늘의 만나", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            StatusBar.Activity = this;

            base.OnCreate(savedInstanceState);

            Forms.SetFlags("FastRenderers_Experimental");

            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(Application);

            Forms.Init(this, savedInstanceState);

            LoadApplication(new App());

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }
    }
}