using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoAsignacionRecurso
    {
        public int id { get; set; }
        
        public string recurso { get; set; }
        
        public List<DtoDescripcion> descripcion { get; set; }

        [JsonProperty(PropertyName = "fecha_arribo")]
        public DateTime? fechaArribo { get; set; }

        [JsonProperty(PropertyName = "actualmente_asignado")]
        public bool actualmenteAsignado { get; set; }
    }
}
