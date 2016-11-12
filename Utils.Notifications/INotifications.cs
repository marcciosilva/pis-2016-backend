namespace Utils.Notifications
{
    public interface INotifications
    {
        /// <summary>
        /// Metodo a sobreescribir por la clase concreta para enviar una notificacion push.
        /// </summary>
        /// <param name="cod">Codigo definido en Codigos.</param>
        /// <param name="pk">Primary Key del elemento que se desea realizar una notificacion.</param>
        /// <param name="topic">Nombre del topic / channel que se desea enviar la notificacion.</param>
        void SendMessage(string cod, int evento, int extension, int zona, string zonaNombre, string topic);

        /// <summary>
        /// Remueve del topic al usuario con tokenFirebase como registration token.
        /// </summary>
        /// <param name="tokenFirebase">Token generado por Firebase y obtenido desde le dispositvo.</param>
        /// <param name="topic">Nombre del canal de publicacion.</param>
        void RemoveUserFromTopic(string tokenFirebase, string topic, string nombreUsuario);
    }
}
