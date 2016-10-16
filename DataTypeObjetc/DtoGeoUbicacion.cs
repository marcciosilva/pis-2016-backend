using System;

namespace DataTypeObject
{
    public class DtoGeoUbicacion
    {
        public int idExtension { get; set; }

        public string usuario { get; set; }

        public DateTime fecha_envio { get; set; }

        public double longitud { get; set; }

        public double latitud { get; set; }       
    }
}