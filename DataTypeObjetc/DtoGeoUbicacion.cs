using Newtonsoft.Json;
using System;

namespace DataTypeObject
{
    public class DtoGeoUbicacion
    {
        public int idExtension { get; set; }

        public string usuario { get; set; }

        [JsonProperty(PropertyName = "fecha_envio")]
        public DateTime fechaEnvio { get; set; }

        public double longitud { get; set; }

        public double latitud { get; set; }       
    }
}