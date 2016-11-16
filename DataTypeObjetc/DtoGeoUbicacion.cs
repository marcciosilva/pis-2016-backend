using System;
using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoGeoUbicacion
    {
        [JsonProperty(PropertyName = "id_extension")]
        public int idExtension { get; set; }

        public string usuario { get; set; }

        [JsonConverter(typeof(FormatoDate))]
        [JsonProperty(PropertyName = "fecha_envio")]
        public DateTime fechaEnvio { get; set; }

        public double longitud { get; set; }

        public double latitud { get; set; }       
    }
}