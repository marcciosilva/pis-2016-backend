using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PubNubMessaging.Core;

namespace Utils.Notifications
{
    public class NotificationsPubNub : INotifications
    {
        private static string PublishKey = "pub-c-a024c6ed-96da-40cd-8e1a-45279bd4b63b";
        private static string SuscribeKey = "sub-c-7d3f5b56-6e54-11e6-9259-0619f8945a4f";

        private static Pubnub _pubnubCliente;

        public NotificationsPubNub()
        {
            _pubnubCliente = new Pubnub(PublishKey, SuscribeKey);
        }



        /// <summary>
        /// Channel names are UTF-8 compatible. Prohibited chars in a channel name are:
        /// comma: ,
        /// slash: /
        /// backslash: \
        /// period: .
        /// asterisks: *
        /// colon: :
        /// </summary>
        /// <param name="channelName"></param>
        public void SubscribeChanel(string channelName)
        {
            _pubnubCliente.Subscribe<string>(channel: channelName, subscribeCallback: DisplaySubscribeReturnMessage, 
                connectCallback: DisplaySubscribeConnectStatusMessage, errorCallback: DisplayErrorMessage);
        }

        public void UnsubscribeChanel(string channelName)
        {
            _pubnubCliente.Unsubscribe<string>(channel: channelName, subscribeCallback: DisplayReturnMessage, connectCallback: DisplaySubscribeConnectStatusMessage
                 , disconnectCallback: DisplaySubscribeDisconnectStatusMessage, errorCallback: DisplayErrorMessage);
        }

        public void SendMessage(string channelName, string message)
        {
            _pubnubCliente.Publish<string>(channel: channelName, message: message, userCallback: DisplayReturnMessage, errorCallback: DisplayErrorMessage);
        }


        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// 


        //public NotificationsPubNub()
        //{
        //    _pubnubCliente = new Pubnub(PublishKey, SuscribeKey);
        //    _pubnubCliente.Subscribe<string>(
        //channel: "Channel-15t1x43ar",
        //subscribeCallback: DisplaySubscribeReturnMessage,
        //connectCallback: DisplaySubscribeConnectStatusMessage,
        //errorCallback: DisplayErrorMessage);
        //}

        public void DisplaySubscribeDisconnectStatusMessage(string var)
        {
           // Console.WriteLine("Estado de desconexion: " + var);
        }
        public void DisplaySubscribeConnectStatusMessage(string connectMessage)
        {
           //Console.WriteLine("Estado de suscripcion: "+connectMessage);
            //_pubnubCliente.Publish<string>(
            //    channel: "Channel-15t1x43ar",
            //    message: "Soy Andres",
            //    userCallback: DisplayReturnMessage,
            //    errorCallback: DisplayErrorMessage);
        }

        void DisplaySubscribeReturnMessage(string result)
        {
            //Console.WriteLine("SUBSCRIBE REGULAR CALLBACK:");
            //Console.WriteLine("Resultado de suscribirse: "+result);
            if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(result.Trim()))
            {
                List<object> deserializedMessage = _pubnubCliente.JsonPluggableLibrary.DeserializeToListOfObject(result);
                if (deserializedMessage != null && deserializedMessage.Count > 0)
                {
                    object subscribedObject = (object)deserializedMessage[0];
                    if (subscribedObject != null)
                    {
                        //IF CUSTOM OBJECT IS EXCEPTED, YOU CAN CAST THIS OBJECT TO YOUR CUSTOM CLASS TYPE
                        string resultActualMessage = _pubnubCliente.JsonPluggableLibrary.SerializeToJsonString(subscribedObject);
                    }
                }
            }
        }

        void DisplayErrorMessage(PubnubClientError pubnubError)
        {
          //  Console.WriteLine(pubnubError.StatusCode);
        }

        void DisplayReturnMessage(string result)
        {
            //Console.WriteLine("PUBLISH STATUS CALLBACK");
           // Console.WriteLine(result);
        }


    }
}
