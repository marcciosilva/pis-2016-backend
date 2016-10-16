using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{
    public class DtoItemListar
    {        
        public int id_evento { get; set; }

        public DtoZona zona { get; set; }
        
        public string descripcion { get; set; }

        public string despachador { get; set; }

        public string estado { get; set; }

        public DateTime fecha_creacion { get; set; }

        public DtoCategoria categoria { get; set; }

        public DtoGeoUbicacion geoubicacion { get; set; }
    }
}
