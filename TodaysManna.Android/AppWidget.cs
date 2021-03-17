using System;
using Android.App;
using Android.App.Job;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using TodaysManna.Models;
using TodaysManna.ViewModel;

namespace TodaysManna.Droid
{
    [BroadcastReceiver(Label = "오늘의 만나")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/appwidget_provider")]
    public class AppWidget : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);

            appWidgetManager.UpdateAppWidget(me, BuildRemoteViews(context, appWidgetIds));
        }
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action.Equals("android.appwidget.action.APPWIDGET_UPDATE"))
            {
                var widgetView = new RemoteViews(context.PackageName, Resource.Layout.Widget);

                WidgetService.SetRangeOnWidgetAsync();
                WidgetService.widgetReady += (s, e) =>
                {
                    widgetView.SetTextViewText(Resource.Id.widgetMedium, WidgetService.WidgetString);

                    var piBackground = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
                    widgetView.SetOnClickPendingIntent(Resource.Id.widget_background, piBackground);

                    AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);
                    var me = new ComponentName(context, Java.Lang.Class.FromType(typeof(AppWidget)).Name);
                    appWidgetManager.UpdateAppWidget(me, widgetView);
                };
            }
        }

        RemoteViews BuildRemoteViews(Context context, int[] appWidgetIds)
        {
            var widgetView = new RemoteViews(context.PackageName, Resource.Layout.Widget);

            RegisterClicks(context, appWidgetIds, widgetView);

            return widgetView;
        }

        private void RegisterClicks(Context context, int[] appWidgetIds, RemoteViews widgetView)
        {
            var intent = new Intent(context, typeof(AppWidget));
            intent.SetAction(AppWidgetManager.ActionAppwidgetUpdate);
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);

            // Register click event for the Background
            var piBackground = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent);
            widgetView.SetOnClickPendingIntent(Resource.Id.widget_background, piBackground);
        }
    }
}
