using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{

    public class DtoAccionesRecursoExtension
    {
        public int id { get; set; }
        
        public string recurso { get; set; }
        
        public string descripcion { get; set; }

        public DateTime fecha_arribo { get; set; }

        public bool actualmente_asignado { get; set; }

    }
}
