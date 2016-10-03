using Ibt.Ortc.Api.Extensibility;
using System;

namespace Utils.Notifications
{
    public class NotificationsRealtime : INotifications
    {

        private OrtcClient _ortcClient;
        private string _channelName;

        public NotificationsRealtime()
        {
            var api = new Ibt.Ortc.Api.Ortc();
            IOrtcFactory factory = api.LoadOrtcFactory("IbtRealTimeSJ");
            _ortcClient = factory.CreateClient();
            _ortcClient.ClusterUrl = "http://ortc-developers.realtime.co/server/2.1/";
            _ortcClient.ConnectionMetadata = "testConnectionMetadata";
            // Se asignan métodos de callback para manejar conexión exitosa
            // y suscripción exitosa a canales.
            _ortcClient.OnConnected += new OnConnectedDelegate(OrtcOnConnected);
            _ortcClient.OnSubscribed += new OnSubscribedDelegate(OrtcOnSubscribed);
            // Se establece clave de aplicación y token de autenticación.
            _ortcClient.Connect("HDzwHh", "myToken");
            // Se setea channelName y message temporales.
            _channelName = "";
        }

       

        /// <summary>
        /// Método utilizado para enviar un mensaje a un canal en particular.
        /// </summary>
        /// <param name="channelName">Nombre de canal.</param>
        /// <param name="message">Mensaje.</param>
        public void SendMessage(string channelName, string message)
        {
            // Si el nombre de canal es distinto al actual, se desuscribe del
            // canal actual y se genera la suscripción al nuevo.
            if (!_channelName.Equals(channelName, StringComparison.Ordinal))
            {
                PrintDebugMessage(@"No se está suscrito a ese canal.");
            } else
            {
                _ortcClient.Send(channelName, message);
            }
        }



        /// <summary>
        /// Se genera la suscripción por parte del cliente a un canal de comunicación
        /// en concreto.
        /// </summary>
        /// <param name="chanelName">Nombre del canal.</param>
        public void SubscribeChanel(string channelName)
        {
            if (_channelName.Equals(channelName, StringComparison.Ordinal))
            {
                PrintDebugMessage("Ya existe una suscripción al canal " 
                    + channelName + ".");
            }
            else
            {
                // Si se está suscrito a un canal, se anula la suscripción
                if (!_channelName.Equals("", StringComparison.Ordinal))
                {
                    PrintDebugMessage(@"Debe desuscribirse de un canal antes de intentar
                        suscribirse a uno nuevo.");
                }
                else
                {
                    _channelName = channelName;
                    _ortcClient.Subscribe(_channelName, true, OnMessageCallback);
                }
            }
        }

        /// <summary>
        /// Se elimina la suscripción de un cliente a un canal en particular.
        /// </summary>
        /// <param name="channelName">Nombre del canal.</param>
        public void UnsubscribeChanel(string channelName)
        {
            _channelName = "";
            _ortcClient.Unsubscribe(channelName);
        }

        /// <summary>
        /// Método llamado cuando el cliente de Realtime Framework consigue
        /// establecer la conexión.
        /// </summary>
        /// <param name="sender"></param>
        private void OrtcOnConnected(object sender)
        {
            _ortcClient.Subscribe(_channelName, true, OnMessageCallback);
        }

        /// <summary>
        /// Método llamado cuando se recibe un mensaje por el canal al que
        /// el cliente está suscrito, sirve para manejarlo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="channel">Canal de comunicación utilizado.</param>
        /// <param name="message">Mensaje recibido.</param>
        private void OnMessageCallback(object sender, string channel, string message)
        {
            Console.WriteLine(channel + " > " + message);
        }

        /// <summary>
        /// Método llamado cuando el cliente consiguió subscribirse
        /// exitosamente a un canal de comunicación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="channel"></param>
        private void OrtcOnSubscribed(object sender, string channel)
        {
            // Sending Hello World message
            _ortcClient.Send(channel, "Backend de Realtime Framework conectado.");
        }

        private void PrintDebugMessage(string msg)
        {
            Console.WriteLine(msg);
        }

    }
}