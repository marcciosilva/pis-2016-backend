﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoInfoCreacionEvento
    {
        [JsonProperty(PropertyName = "zonas_sectores")]
        public ICollection<DtoZona> zonasSectores { get; set; }

        public ICollection<DtoCategoria> categorias { get; set; }

        public ICollection<DtoDepartamento> departamentos { get; set; }
    }
}