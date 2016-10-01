using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{
    public enum EstadoExtension
    {
        FaltaDespachar,
        Despachado,
        Cerrado
    }

    public class DtoExtension
    {        
        public DtoZona zona { get; set; }

        public string despachador { get; set; }

        public string descripcion_despachador { get; set; }

        public string descripcion_supervisor { get; set; }

        public EstadoExtension estado { get; set; }

        public DateTime time_stamp { get; set; }

        public DtoCategoria segunda_categoria { get; set; }

        public ICollection<string> recursos { get; set; }

    }
}
