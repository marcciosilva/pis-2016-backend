namespace Utils.Notifications
{
    public static class FactoryNotifications
    {
        /// <summary>
        /// Enumerado de las opciones manejadas por la factory.
        /// </summary>
        public enum PushNotificationsSystem
        {
            Firebase
        }

        // Determina qué sistema se utiliza para push notifications.
        private static PushNotificationsSystem _currentNotificationSystem = PushNotificationsSystem.Firebase;

        private static INotifications _singleNotificationInstance = null;

        /// <summary>
        /// Obtiene la clase concreta para la Factory de notificaciones.
        /// </summary>
        /// <returns>Devulve la clase concreta para la instancia de INotificactions.</returns>
        public static INotifications GetInstance()
        {
            if (_currentNotificationSystem == PushNotificationsSystem.Firebase)
            {
                if (_singleNotificationInstance != null)
                {
                    return _singleNotificationInstance;
                }
                else
                {
                    _singleNotificationInstance = new NotificationsFirebase();
                    return _singleNotificationInstance;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
