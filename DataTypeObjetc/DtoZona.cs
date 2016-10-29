﻿using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoZona
    {
        public int id { get; set; }

        public string nombre { get; set; }

        public string nombre_ue { get; set; }

        public ICollection<DtoSector> sectores { get; set; }
    }
}