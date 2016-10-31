using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoZona
    {
        public int id { get; set; }

        public string nombre { get; set; }

        [JsonProperty(PropertyName = "nombre_ue")]
        public string nombreUe { get; set; }

        public ICollection<DtoSector> sectores { get; set; }
    }
}