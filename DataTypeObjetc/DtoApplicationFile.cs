using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoApplicationFile
    {        
        public string nombre { get; set; }

        [JsonProperty(PropertyName = "file_data")]
        public byte[] fileData { get; set; }

        [JsonProperty(PropertyName = "id_extension")]
        public int idExtension { get; set; }
    }
}