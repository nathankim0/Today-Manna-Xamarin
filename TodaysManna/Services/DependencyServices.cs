using System;
using System.Collections.Generic;

namespace TodaysManna
{
    public interface IKeyboardHelper
    {
        void HideKeyboard();
    }

    public interface IEventTracker
    {
        void SendEvent(string eventId);
        void SendEvent(string eventId, string paramName, string value);
        void SendEvent(string eventId, IDictionary<string, string> parameters);
    }

    public interface ILocalNotification
    {
        //void ShowNotification(string strTitle, string strDescription, string idNotification, string strURL);
        void ShowNotification(
            string strNotificationTitle,
            string strNotificationSubtitle,
            string strNotificationDescription,
            string strNotificationIdItem,
            string strDateOrInterval,
            bool isInterval,
            string extraParameters);

        void DisableNotification();
    }

    public interface IClearCookies
    {
        void Clear();
    }
}
