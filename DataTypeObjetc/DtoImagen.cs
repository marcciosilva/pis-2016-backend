using System;
using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoImagen
    {
        public int id { get; set; }

        [JsonProperty(PropertyName = "id_imagen")]
        public int idImagen { get; set; }

        public string usuario { get; set; }

        [JsonConverter(typeof(FormatoDate))]
        [JsonProperty(PropertyName = "fecha_envio")]
        public DateTime fechaEnvio { get; set; }

        public int idExtension { get; set; }
    }
}