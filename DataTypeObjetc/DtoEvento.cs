using System;
using System.Collections.Generic;

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

        public string creador { get; set; }
        
        public DateTime fecha_creacion { get; set; }     

        public string calle { get; set; }
        
        public string esquina { get; set; }
        
        public string numero { get; set; }

        public string departamento { get; set; }

        public string sector { get; set; }

        public double longitud { get; set; }

        public double latitud { get; set; }

        public string descripcion { get; set; }

        public bool en_proceso { get; set; }              
        
        public ICollection<DtoExtension> extensiones { get; set; }

        //public DtoOrigenEvento origen_evento { get; set; }

        public virtual ICollection<DtoImagen> imagenes { get; set; }

        public virtual ICollection<DtoVideo> videos { get; set; }

        public virtual ICollection<DtoAudio> audios { get; set; }

        //public virtual ICollection<DtoGeoUbicacion> geo_ubicaciones { get; set; }
    }
}
