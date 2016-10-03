using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Notifications
{
    public interface INotifications
    {
        void SubscribeChanel(string channelName);
        void UnsubscribeChanel(string channelName);
        void SendMessage(string channelName, string message);
    }
}
