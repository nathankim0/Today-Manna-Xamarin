using System;
using UserNotifications;
using Foundation;
using static CoreText.CTFontFeatureAllTypographicFeatures;
using TodaysManna.iOS.AppDependencies;
using TodaysManna.AppInterfaces;

[assembly: Xamarin.Forms.Dependency(typeof(LocalNotification))]
namespace TodaysManna.iOS.AppDependencies
{
    public class LocalNotification : ILocalNotification
    {
        public void ShowNotification(string strNotificationTitle,
                                string strNotificationSubtitle,
                                string strNotificationDescription,
                                string strNotificationIdItem,
                                string strDateOrInterval,
                                bool isInterval,
                                string extraParameters)
        {
            //intervalType: 1 - set to date | 2 - set to interval



            //Object creation.
            var notificationContent = new UNMutableNotificationContent();


            //Set parameters.
            //notificationContent.Title = strNotificationTitle;
            //notificationContent.Subtitle = strNotificationSubtitle;
            notificationContent.Body = strNotificationDescription;
            //notificationContent.Badge = 1;
            notificationContent.Badge = Int32.Parse(strNotificationIdItem);
            notificationContent.Sound = UNNotificationSound.Default;


            NSDateComponents notificationContentNSCDate=null;

            try
            {
                //Set date.
                DateTime notificationContentDate = Convert.ToDateTime(strDateOrInterval);

                notificationContentNSCDate = new NSDateComponents
                {
                    Year = notificationContentDate.Year,
                    Month = notificationContentDate.Month,
                    Day = notificationContentDate.Day,
                    Hour = notificationContentDate.Hour,
                    Minute = notificationContentDate.Minute,
                    Second = notificationContentDate.Second,
                    Nanosecond = (notificationContentDate.Millisecond * 1000000),
                    TimeZone = NSTimeZone.SystemTimeZone
                };
            }
            catch
            {

            }


            //Set trigger and request.
            var notificationRequestID = strNotificationIdItem;
            UNNotificationRequest notificationRequest = null;

            if (isInterval)
            {
                var notificationCalenderTrigger = UNTimeIntervalNotificationTrigger.CreateTrigger(double.Parse(strDateOrInterval), true);

                notificationRequest = UNNotificationRequest.FromIdentifier(notificationRequestID, notificationContent, notificationCalenderTrigger);
            }
            else
            {

                var notificationIntervalTrigger = UNCalendarNotificationTrigger.CreateTrigger(notificationContentNSCDate, true);

                notificationRequest = UNNotificationRequest.FromIdentifier(notificationRequestID, notificationContent, notificationIntervalTrigger);
            }


            //Add the notification request.
            UNUserNotificationCenter.Current.AddNotificationRequest(notificationRequest, (err) =>
            {
                if (err != null)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + err);
                }
            });
        }
        public void DisableNotification()
        {
            UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
        }
    }
}
