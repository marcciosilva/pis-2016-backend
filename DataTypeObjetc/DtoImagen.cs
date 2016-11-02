using System;
using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoImagen
    {
        public int id { get; set; }

        public int id_imagen { get; set; }

        public string usuario { get; set; }

        [JsonProperty(PropertyName = "fecha_envio")]
        public DateTime fechaEnvio { get; set; }

        public int idExtension { get; set; }
    }
}