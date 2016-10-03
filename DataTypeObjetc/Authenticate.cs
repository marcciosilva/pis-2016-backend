using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObjetc
{

    public class Authenticate : Response
    {
        public string access_token { get; set; }
        public string msg { get; set; }

        public Authenticate(string token, string  mensaje)
        {
            access_token = token;
            msg = mensaje;
        }
    }
}
