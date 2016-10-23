using System;

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

        public string descripcion { get; set; }

        public OrigenDescripcion origen { get; set; }
    }
}
