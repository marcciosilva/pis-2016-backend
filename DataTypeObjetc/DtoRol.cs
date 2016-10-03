using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoRol
    {
        public ICollection<DtoZona> zonas { get; set; }

        public ICollection<DtoRecurso> recursos { get; set; }

    }
}