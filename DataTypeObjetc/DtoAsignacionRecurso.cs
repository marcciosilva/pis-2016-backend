using System;
using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoAsignacionRecurso
    {
        public int id { get; set; }
        
        public string recurso { get; set; }
        
        public List<DtoDescripcion> descripcion { get; set; }

        public DateTime? fecha_arribo { get; set; }

        public bool actualmente_asignado { get; set; }
    }
}
