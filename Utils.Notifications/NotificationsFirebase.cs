using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using DataTypeObject;
using Newtonsoft.Json;

namespace Utils.Notifications
{
    class NotificationsFirebase : INotifications
    {
        public void SendMessage(string cod, string pk, string topic)
        {
            sendNotification(cod, pk, topic);
        }

        public void SubscribeChanel(string channelName)
        {
            //throw new NotImplementedException();
        }

        public void UnsubscribeChanel(string channelName)
        {
            //throw new NotImplementedException();
        }

        private async void sendNotification(string cod, string pk,  string topic)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://fcm.googleapis.com/fcm/send"),
                    Method = HttpMethod.Post,
                    

                };
                string keyFireBase = WebConfigurationManager.AppSettings["KeyFireBase"];

                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key = AIzaSyBqSkDDnIA_IVu2IyA6G7ywL7eiFXF5cfs");//"key ="+ keyFireBase);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var notificationJSon = "{\"notification\":{\"title\":\"Notificacion ejemplo\", \"text\":\"" 
                //    + message + "\"},\"to\":\"eWWad4fhkJQ:APA91bEukKiqr1Pk4my17Qtyc-vgHUohyUx5wwsm9JiAWE-PJs0Pxo0JkW34fpe6oYIpJdoYb6pWzyFT6s1UtNeQP0LNa3AUL-2dJVTsl32Ntss82pDKR39h0fKV-ONazAIaKnDmaaPG\"}";
                topic = "/topics/" + topic;
                var notificationJSon = JsonConvert.SerializeObject(new Notificacion(topic, new data(cod, pk)) );
                var content = new StringContent(notificationJSon, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();//{"message_id":6526277793921178237}
                if (!response.IsSuccessStatusCode) {
                    Emsys.Logs.Log.AgregarLogError("vacio", "servidor", "Utils.Notitications", "NotificacionesFirebase", 0, "sendNotification", "Se genero una notificacion al topic: " + topic + "con el codigo: " + cod + "y la pk:" + pk + " la respuesta de firebase es: " + responseString + " y la respuesta fue: " + response.ToString(), Mensajes.LogNotificacionesCod);
                    return;
                }
                string mensaje = responseString.Split(':')[0].ToString();
                if (mensaje  != "{\"message_id\"") {
                    Emsys.Logs.Log.AgregarLogError("vacio", "servidor", "Utils.Notitications", "NotificacionesFirebase", 0, "sendNotification", "Se genero una notificacion al topic: " + topic + "con el codigo: " + cod + "y la pk:" + pk + " la respuesta de firebase es: " + responseString + " y la respuesta fue: " + response.ToString(), Mensajes.LogNotificacionesCod);
                    return;
                }

            }
            Emsys.Logs.Log.AgregarLog("vacio", "servidor", "Utils.Notitications", "NotificacionesFirebase", 0, "sendNotification", "Se genero una notificacion al topic: "+ topic+"con el codigo: "+ cod + "y la pk:" + pk, Mensajes.LogNotificacionesCod);

        }
    }
}
