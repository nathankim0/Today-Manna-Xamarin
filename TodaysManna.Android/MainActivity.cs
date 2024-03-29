﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using Android.Content;

namespace TodaysManna.Droid
{
    [Activity(Label = "오늘의 만나", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", Theme = "@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            StatusBar.Activity = this;

            base.OnCreate(savedInstanceState);

            Forms.SetFlags("FastRenderers_Experimental");

            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(Application);

            NotificationCenter.CreateNotificationChannel();

            Forms.Init(this, savedInstanceState);

            LoadApplication(new App());

            NotificationCenter.NotifyNotificationTapped(Intent);

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

        protected override void OnNewIntent(Intent intent)
        {
            NotificationCenter.NotifyNotificationTapped(intent);
            base.OnNewIntent(intent);
        }
    }
}
