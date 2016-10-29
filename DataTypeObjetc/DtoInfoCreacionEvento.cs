using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoInfoCreacionEvento
    {
        public ICollection<DtoZona> zonas_sectores { get; set; }

        public ICollection<DtoCategoria> categorias { get; set; }

        public ICollection<DtoDepartamento> departamentos { get; set; }
    }
}