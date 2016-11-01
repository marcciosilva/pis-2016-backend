namespace DataTypeObject
{
    public class Notificacion
    {
        public Notificacion(string channel,  data d)
        {
            this.to = channel;
            this.data = d;
        }
        public string to { get; set; }

        public data data { get; set; }
    }
}
