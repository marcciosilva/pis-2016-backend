using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoActualizarDescripcionOffline
    {
        public string descripcion { get; set; }

        [JsonProperty(PropertyName = "id_extension")]
        public int idExtension { get; set; }

        [JsonProperty(PropertyName = "user_data")]
        public DtoUsuario userData { get; set; }
    }
}
