using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Notifications
{
    class NotificationsFirebase : INotifications
    {
        public void SendMessage(string channelName, string message)
        {
            sendNotification(message);
        }

        public void SubscribeChanel(string channelName)
        {
            //throw new NotImplementedException();
        }

        public void UnsubscribeChanel(string channelName)
        {
            //throw new NotImplementedException();
        }

        private async void sendNotification(string message)
        {
            using (var client = new HttpClient())
            {

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://fcm.googleapis.com/fcm/send"),
                    Method = HttpMethod.Post
                };

                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=AIzaSyBqSkDDnIA_IVu2IyA6G7ywL7eiFXF5cfs");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var notificationJSon = "{\"notification\":{\"title\":\"Notificacion ejemplo\", \"text\":\"" 
                    + message + "\"},\"to\":\"eWWad4fhkJQ:APA91bEukKiqr1Pk4my17Qtyc-vgHUohyUx5wwsm9JiAWE-PJs0Pxo0JkW34fpe6oYIpJdoYb6pWzyFT6s1UtNeQP0LNa3AUL-2dJVTsl32Ntss82pDKR39h0fKV-ONazAIaKnDmaaPG\"}";

                var content = new StringContent(notificationJSon, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);

                var responseString = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
