using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils.Notifications;
namespace Emsys.ObserverDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            var cliente = FactoryNotifications.GetInstance();
            string channel = "myChannel";
            cliente.SubscribeChanel(channel);
            while (true)
            {
                Console.Write("> ");
                string msg = Console.ReadLine();
                cliente.SendMessage(channel, msg);
            }
        }
    }
}
