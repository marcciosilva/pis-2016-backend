using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoOrigenEvento
    {
        public int id { get; set; }

        [JsonProperty(PropertyName = "id_origen")]
        public int idOrigen { get; set; }

        [JsonProperty(PropertyName = "tipo_origen")]
        public string tipoOrigen { get; set; }
    }
}