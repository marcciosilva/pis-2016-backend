using System;
using System.Collections.Generic;

namespace DataTypeObject
{
    public class DtoExtension
    {        
        public int id { get; set; }

        public DtoZona zona { get; set; }

        public string despachador { get; set; }
        
        public List<DtoDescripcion> descripcion_despachadores { get; set; }

        public string descripcion_supervisor { get; set; }

        public ICollection<DtoAsignacionRecurso> asignaciones_recursos { get; set; }

        public string estado { get; set; }

        public DateTime time_stamp { get; set; }

        public DtoCategoria segunda_categoria { get; set; }

        public ICollection<string> recursos { get; set; }

        public virtual ICollection<DtoImagen> imagenes { get; set; }

        public virtual ICollection<DtoVideo> videos { get; set; }

        public virtual ICollection<DtoAudio> audios { get; set; }

        public virtual ICollection<DtoGeoUbicacion> geo_ubicaciones { get; set; }
    }
}
