﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoRecursosExtension
    {
        [JsonProperty(PropertyName = "id_extension")]
        public int idExtension { get; set; }

        [JsonProperty(PropertyName = "recursos_asignados")]
        public ICollection<DtoRecurso> recursosAsignados { get; set; }

        [JsonProperty(PropertyName = "recursos_no_asignados")]
        public ICollection<DtoRecurso> recursosNoAsignados { get; set; }
    }
}