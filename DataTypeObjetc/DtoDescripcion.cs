using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{
    public enum OrigenDescripcion
    {
        Recurso,
        Despachador
    }

    public class DtoDescripcion
    {
        public string usuario { get; set; }

        public string texto { get; set; }

        public DateTime fecha { get; set; }

        public OrigenDescripcion origen { get; set; }
    }
}
