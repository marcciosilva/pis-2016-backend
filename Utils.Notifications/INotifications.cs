using DataTypeObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Notifications
{
    public interface INotifications
    {
        //void SubscribeChanel(string channelName);
        //void UnsubscribeChanel(string channelName);
        /// <summary>
        /// Metodo a sobreescribir por la clase concreta para enviar una notificacion push.
        /// </summary>
        /// <param name="cod">Codigo definido en Codigos.</param>
        /// <param name="pk">Primary Key del elemento que se desea realizar una notificacion.</param>
        /// <param name="topic">Nombre del topic / channel que se desea enviar la notificacion.</param>
        void SendMessage(string cod, string pk, string topic);
    }
}
