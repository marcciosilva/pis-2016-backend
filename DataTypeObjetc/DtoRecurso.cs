using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoRecurso
    {
        public int id { get; set; }

        public string codigo { get; set; }

        public string estado { get; set; }

        [JsonProperty(PropertyName = "estado_asignacion")]
        public string estadoAsignacion { get; set; }

        public string usuario { get; set; }
    }
}