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
        public string Zona { get; set; }

        public string Despachador { get; set; }

        public string DescripcionDespachador { get; set; }

        public string DescripcionSupervisor { get; set; }

        public EstadoExtension Estado { get; set; }

        public DateTime TimeStamp { get; set; }

        public DtoCategoria SegundaCategoria { get; set; }

        public ICollection<string> Recursos { get; set; }

    }
}
