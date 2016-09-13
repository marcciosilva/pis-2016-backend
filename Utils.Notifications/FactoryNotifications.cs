using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Notifications
{
    public static class FactoryNotifications
    {

        public enum PushNotificationsSystem { PubNub, RealtimeFramework };

        // Determina qué sistema se utiliza para push notifications.
        private static PushNotificationsSystem _currentNotificationSystem = PushNotificationsSystem.RealtimeFramework;

        public static INotifications GetInstance() {
            if (_currentNotificationSystem == PushNotificationsSystem.PubNub)
            {
                return new NotificationsPubNub();
            }
            else if (_currentNotificationSystem == PushNotificationsSystem.RealtimeFramework)
            {
                return new NotificationsRealtime();
            }
            else
            {
                return null;
            }
        }
    }
}
