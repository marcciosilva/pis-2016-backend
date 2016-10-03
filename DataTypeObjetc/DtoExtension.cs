using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{

    public class DtoExtension
    {        
        public DtoZona zona { get; set; }
        
        public string descripcion { get; set; }

        //public string descripcion_supervisor { get; set; }

        public string estado { get; set; }

        public DateTime time_stamp { get; set; }

        public DtoCategoria categoria { get; set; }

        public ICollection<string> recursos { get; set; }

    }
}
