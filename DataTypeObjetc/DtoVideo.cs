using Newtonsoft.Json;
using System;

namespace DataTypeObject
{
    public class DtoVideo
    {
        public int id { get; set; }

        [JsonProperty(PropertyName = "id_video")]
        public int idVideo { get; set; }

        public string usuario { get; set; }

        [JsonProperty(PropertyName = "fecha_envio")]
        public DateTime fechaEnvio { get; set; }

        [JsonProperty(PropertyName = "id_extension")]
        public int idExtension { get; set; }
    }
}