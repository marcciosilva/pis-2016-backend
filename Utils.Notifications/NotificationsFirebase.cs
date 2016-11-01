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
using Emsys.LogicLayer;
using System.Threading;

namespace Utils.Notifications
{
    class NotificationsFirebase : INotifications
    {
        private Semaphore _pool = new Semaphore(1, 1);
        private int _seconds = Convert.ToInt32(WebConfigurationManager.AppSettings["TiempoEsperaEnvioNotificaciones"]);
        /// <summary>
        /// Implementacion on FireBase del metodo Send para enviar notificaciones push.
        /// </summary>
        /// <param name="cod">Codigo definido en Codigos para enviar una notificacion.</param>
        /// <param name="pk">Primary Key del elemento que se realizo la notificacion y se desea enviar.</param>
        /// <param name="topic">Topic/Channel al que se desea enviar una notificacion.</param>
        public void SendMessage(string cod, string pk, string topic)
        {
            sendNotification(cod, pk, topic);
        }

        /// <summary>
        /// Implementacion para enviar de forma asyncronica.
        /// </summary>
        /// <param name="cod">Codigo del mensaje a enviar definio en Codigos.</param>
        /// <param name="pk">Primary Key de elemento a informar que cambio.</param>
        /// <param name="topic">Topic/Channel de elemento que fue modificado.</param>
        private async void sendNotification(string cod, string pk, string topic)
        {
            using (var client = new HttpClient())
            {
                IMetodos dbAL = new Metodos();
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://fcm.googleapis.com/fcm/send"),
                    Method = HttpMethod.Post,
                };
                string keyFireBase = WebConfigurationManager.AppSettings["KeyFireBase"];

                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key = " + keyFireBase);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var topicFinal = "/topics/" + topic;
                var notificationJSon = JsonConvert.SerializeObject(new Notificacion(topicFinal, new data(cod, pk)));
                var content = new StringContent(notificationJSon, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    dbAL.AgregarLogErrorNotification("vacio", "servidor", "Utils.Notitications", "NotificacionesFirebase", 0, "sendNotification", "Se genero una notificacion al topic: " + topicFinal + " con el codigo: " + cod + " y la pk: " + pk + " la respuesta de firebase es: " + responseString + " y la respuesta fue: " + response.ToString(), MensajesParaFE.LogNotificacionesCod);
                    throw new Exception("Al enviar una notifiacion la respuesta del servidor NO fue positiva.");
                }
                string mensaje = responseString.Split(':')[0].ToString();
                if (mensaje != "{\"message_id\"")
                {
                    dbAL.AgregarLogErrorNotification("vacio", "servidor", "Utils.Notitications", "NotificacionesFirebase", 0, "sendNotification", "Se genero una notificacion al topic: " + topicFinal + " con el codigo: " + cod + " y la pk: " + pk + " la respuesta de firebase es: " + responseString + " y la respuesta fue: " + response.ToString(), MensajesParaFE.LogNotificacionesCod);
                    _pool.WaitOne();
                    Thread.Sleep(_seconds * 1000);
                    _pool.Release();
                    sendNotification(cod, pk, topic);
                    // throw new Exception("Al enviar una notifiacion la respuesta del servidor NO contiene el id del mensjae, entonces la respuesta es negativa.");
                }

                dbAL.AgregarLogNotification("vacio", "servidor", "Utils.Notitications", "NotificacionesFirebase", 0, "sendNotification", "Se genero una notificacion al topic: " + topicFinal + "con el codigo: " + cod + "y la pk:" + pk, MensajesParaFE.LogNotificacionesCod);
            }
        }
    }
}
