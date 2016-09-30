using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoRol
    {
        public ICollection<DtoZona> Zonas { get; set; }

        public ICollection<DtoRecurso> Recursos { get; set; }

    }
}