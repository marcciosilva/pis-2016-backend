﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using DataTypeObject;
using Emsys.DataAccesLayer.Model;
using Newtonsoft.Json;
using Utils.Notifications.Utils;
using System.Linq;

namespace Utils.Notifications
{
    class NotificationsFirebase : INotifications
    {
        /// <summary>
        /// Funcion para remover de la lista de de suscriptos de un canal.
        /// </summary>
        /// <param name="tokenFirebase">Identificador de dispositivo provisto por firebase.</param>
        /// <param name="topic">Canal del que se desea des suscribir.</param>
        /// <param name="nombreUsuario">Nombre del usuario que se desea dessuscribir.</param>
        public async void RemoveUserFromTopic(string tokenFirebase, string topic, string nombreUsuario)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(" https://iid.googleapis.com/iid/v1:batchRemove"),
                    Method = HttpMethod.Post,
                };
                string keyFireBase = WebConfigurationManager.AppSettings["KeyFireBase"];

                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key = " + keyFireBase);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var topicFinal = "/topics/" + topic;
                var BodyRequest = JsonConvert.SerializeObject(new DtoUnsuscribeTokenFirebase(topicFinal, tokenFirebase));
                var content = new StringContent(BodyRequest, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    LogsManager.AgregarLogNotificationDessuscripcionUsuarioError("vacio", "servidor",
                       "Utils.Notitications", "RemoveUserFromTopic", 0,
                       "sendNotification", "Error al enviar mensaje por taza superada",
                       MensajesParaFE.LogNotificacionesDessuscripcionUsuarioTopicErrorGenericoRequest,
                        topicFinal, nombreUsuario, response.ToString());
                   // throw new Exception("Al enviar una notifiacion la respuesta del servidor NO fue positiva.");
                }
                else {

                    //si se pudo quitar entonces lo quito de la bd el token.
                    Emsys.DataAccesLayer.Core.EmsysContext db = new Emsys.DataAccesLayer.Core.EmsysContext();
                    var user = db.Usuarios.Where(x=>x.Nombre == nombreUsuario).FirstOrDefault();
                    user.RegistrationTokenFirebase = null;
                    db.SaveChanges();

                    LogsManager.AgregarLogNotificationDessuscripcionUsuario("vacio", "servidor", "Utils.Notitications",
                        "NotificacionesFirebase", 0, "sendNotification",
                        "Se genero una notificacion exitosamente.",
                        MensajesParaFE.LogNotificacionesDessuscripcionUsuarioTopic,
                        topicFinal, nombreUsuario, responseString);
                }

                //string mensaje = responseString.Split(':')[0].ToString();
                //if (mensaje != "{\"message_id\"")
                //{
                //    LogsManager.AgregarLogNotificationDessuscripcionUsuarioError("vacio", "servidor",
                //       "Utils.Notitications", "RemoveUserFromTopic", 0,
                //       "sendNotification", "Error al enviar mensaje por taza superada",
                //       MensajesParaFE.LogNotificacionesDessuscripcionUsuarioTopicError,
                //        topicFinal, nombreUsuario, response.ToString());
                //    semaforoDessucripcion.WaitOne();
                //    Thread.Sleep(_seconds * 1000);
                //    semaforoDessucripcion.Release();
                //    RemoveUserFromTopic(topic, tokenFirebase, nombreUsuario);
                //    // throw new Exception("Al enviar una notifiacion la respuesta del servidor NO contiene el id del mensjae, entonces la respuesta es negativa.");
                //}
                //else

            }
        }

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
            var log=LogsManager.AgregarLogNotification("vacio", "servidor", "Utils.Notitications", "NotificacionesFirebase",
                0, "sendNotification",
                "Se genero una notificacion Real.",
                MensajesParaFE.LogNotificaciones, topic, cod, pk, "No tengo aun por que no se envio a firebase en este punto.", null);
            //EstadoSistema();
            sendNotification(cod, pk, topic, log);
        }
        

        /// <summary>
        /// Implementacion para enviar de forma asyncronica.
        /// </summary>
        /// <param name="cod">Codigo del mensaje a enviar definio en Codigos.</param>
        /// <param name="pk">Primary Key de elemento a informar que cambio.</param>
        /// <param name="topic">Topic/Channel de elemento que fue modificado.</param>
        /// <param name="logPrevio"></param>
        private async void sendNotification(string cod, string pk, string topic, LogNotification logPrevio = null)
        {
            var topicFinal = "/topics/" + topic;
            try
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri("https://fcm.googleapis.com/fcm/send"),
                        Method = HttpMethod.Post,
                    };
                    string keyFireBase = WebConfigurationManager.AppSettings["KeyFireBase"];

                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key = " + keyFireBase);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var notificationJSon = JsonConvert.SerializeObject(new Notificacion(topicFinal, new data(cod, pk)));
                    var content = new StringContent(notificationJSon, Encoding.UTF8, "application/json");
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        var log = LogsManager.AgregarLogErrorNotification("vacio", "servidor", "Utils.Notitications",
                            "NotificacionesFirebase", 0, "sendNotification",
                            "Ocurrio un error al enviar la notificacion.",
                            MensajesParaFE.LogNotificacionesErrorGenerico, topicFinal, cod, pk,
                            response.ToString(), logPrevio);
                        throw new Exception("Al enviar una notifiacion la respuesta del servidor NO fue positiva.");
                    }

                    string mensaje = responseString.Split(':')[0].ToString();
                    if (mensaje != "{\"message_id\"")
                    {
                        var logActual = LogsManager.AgregarLogErrorNotification("vacio", "servidor",
                            "Utils.Notitications", "NotificacionesFirebase", 0,
                            "sendNotification", "Error al enviar mensaje por taza superada",
                            MensajesParaFE.LogNotificacionesErrorReenvio,
                             topicFinal, cod, pk, response.ToString(), logPrevio);
                        _pool.WaitOne();
                        Thread.Sleep(_seconds * 1000);
                        _pool.Release();
                        sendNotification(cod, pk, topic, logActual);
                    }
                    else
                    {
                        LogsManager.AgregarLogNotification("vacio", "servidor", "Utils.Notitications",
                            "NotificacionesFirebase", 0, "sendNotification",
                            "Se genero una notificacion exitosamente.",
                            MensajesParaFE.LogNotificacionesCierreEnvio,
                            topicFinal, cod, pk, responseString,
                            logPrevio);
                    }
                }
            }
            catch (Exception e)
            {
                var logActual = LogsManager.AgregarLogErrorNotification("vacio", "servidor",
                            "Utils.Notitications", "NotificacionesFirebase", 0,
                            "sendNotification", "Error generico cuando se envio una notificacion: "+e.ToString(),
                            MensajesParaFE.LogNotificacionesErrorGenerico,
                             topicFinal, cod, pk, "no la tengo.", logPrevio);
                _pool.WaitOne();
                throw;
            }
        }
    }
}
