using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Notifications
{
    class NotificationsFirebase : INotifications
    {
        public const string FCM_URL = "https://fcm.googleapis.com/fcm/send";
        public const string GOOGLE_API_KEY = "AIzaSyBqSkDDnIA_IVu2IyA6G7ywL7eiFXF5cfs";
        public const string POST = "POST";
        public const string DEVICE_ID = "eWWad4fhkJQ:APA91bEukKiqr1Pk4my17Qtyc-vgHUohyUx5wwsm9JiAWE-PJs0Pxo0JkW34fpe6oYIpJdoYb6pWzyFT6s1UtNeQP0LNa3AUL-2dJVTsl32Ntss82pDKR39h0fKV-ONazAIaKnDmaaPG";

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
            WebRequest request = WebRequest.Create(FCM_URL);

            // Headers.
            request.Method = POST;
            request.ContentType = "application/json";
            request.Headers.Add(string.Format("Authorization: key={0}", GOOGLE_API_KEY));

            string postData = "{\"data\":{\"title\":\"EMSYS Mobile\", \"text\":\"" + message + "\"},\"to\":\"" + DEVICE_ID + "\"}";
            Console.WriteLine(postData);

            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = request.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();
            Console.WriteLine(sResponseFromServer);

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
        }
    }
}
