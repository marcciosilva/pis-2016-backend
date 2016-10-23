namespace DataTypeObject
{
    public class Notificacion
    {
        public Notificacion(string channel,  data d)
        {
            this.to = channel;
            // registration_ids = firebaseRegistrationId;
            this.data = d;
        }
      // public string registration_ids { get; set; }
        public string to { get; set; }

        public data data { get; set; }
    }
}
