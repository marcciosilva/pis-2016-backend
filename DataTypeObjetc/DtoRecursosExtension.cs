using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoRecursosExtension
    {
        public int idExtension { get; set; }

        public ICollection<DtoRecurso> recursosAsignados { get; set; }

        public ICollection<DtoRecurso> recursosNoAsignados { get; set; }
    }
}