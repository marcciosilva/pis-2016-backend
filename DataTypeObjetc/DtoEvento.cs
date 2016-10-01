using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{
    public class DtoEvento
    {
        public int id { get; set; }
        
        public string informante { get; set; }

        public string telefono { get; set; }

        public DtoCategoria categoria { get; set; }

        public string estado { get; set; }

        public DateTime time_stamp { get; set; }
        
        public DateTime fecha_creacion { get; set; }
        
        public string departamento { get; set; }

        public string calle { get; set; }
        
        public string esquina { get; set; }
        
        public string numero { get; set; }

        public string sector { get; set; }

        public double latitud { get; set; }

        public double longitud { get; set; }

        public string descripcion { get; set; }

        public bool en_proceso { get; set; }

        public ICollection<DtoExtension> extensiones { get; set; }
                
    }
}
