using Newtonsoft.Json;
using System;

namespace DataTypeObject
{
    public class DtoAudio
    {        
        public int id { get; set; }

        [JsonProperty(PropertyName = "id_audio")]
        public int idAudio { get; set; }

        public string usuario { get; set; }

        [JsonProperty(PropertyName = "fecha_envio")]
        public DateTime fechaEnvio { get; set; }

        [JsonProperty(PropertyName = "id_extension")]
        public int idExtension { get; set; }
    }
}