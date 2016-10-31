using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoActualizarDescripcion
    {
        public string descripcion { get; set; }

        [JsonProperty(PropertyName = "id_extension")]
        public int idExtension { get; set; }
    }
}
