using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{
    public class Notificacion
    {
        public Notificacion(string channel,  data d) {
            to = channel;
           // registration_ids = firebaseRegistrationId;
            data = d;
        }
      //  public string registration_ids { get; set; }
        public string  to { get; set; }
        public data data { get; set; }
    }

}
