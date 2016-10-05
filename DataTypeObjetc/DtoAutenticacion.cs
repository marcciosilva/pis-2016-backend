using DataTypeObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObjetc
{

    public class DtoAutenticacion
    {
        public string access_token { get; set; }

        public string msg { get; set; }

        //public DtoRol rol { get; set; }

        public DtoAutenticacion(string token, string mensaje)
        {
            access_token = token;
            msg = mensaje;
        }

        // Comentado por compatibilidad.
        //public DtoAutenticacion(string token, string  mensaje, DtoRol dtorol)
        //{
        //    access_token = token;
        //    msg = mensaje;
        //    rol = dtorol;
        //}
    }
}
