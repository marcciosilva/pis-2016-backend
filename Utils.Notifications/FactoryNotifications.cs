﻿namespace Utils.Notifications
{
    public static class FactoryNotifications
    {
        public enum PushNotificationsSystem
        {
            PubNub,
            RealtimeFramework,
            Firebase
        }

        // Determina qué sistema se utiliza para push notifications.
        private static PushNotificationsSystem _currentNotificationSystem = PushNotificationsSystem.Firebase;

        private static INotifications SingleNotificationInstance = null;

        /// <summary>
        /// Obtiene la clase concreta para la Factory de notificaciones.
        /// </summary>
        /// <returns>Devulve la clase concreta para la instancia de INotificactions.</returns>
        public static INotifications GetInstance()
        {
            ////if (_currentNotificationSystem == PushNotificationsSystem.PubNub)
            ////{
            ////    return new NotificationsPubNub();
            ////}
            ////else if (_currentNotificationSystem == PushNotificationsSystem.RealtimeFramework)
            ////{
            ////    return new NotificationsRealtime();
            ////}
            ////else
            if (_currentNotificationSystem == PushNotificationsSystem.Firebase)
            {
                if (SingleNotificationInstance != null)
                {
                    return SingleNotificationInstance;
                }
                else
                {
                    SingleNotificationInstance = new NotificationsFirebase();
                    return SingleNotificationInstance;
                    // return new NotificationsFirebase(); 
                }
            }
            else
            {
                return null;
            }
        }
    }
}
