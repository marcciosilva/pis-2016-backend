using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoEvento
    {
        public int id { get; set; }
        
        public string informante { get; set; }

        public string telefono { get; set; }

        public DtoCategoria categoria { get; set; }

        public string estado { get; set; }

        [JsonConverter(typeof(FormatoDate))]
        [JsonProperty(PropertyName = "time_stamp")]
        public DateTime timeStamp { get; set; }

        public string creador { get; set; }

        [JsonConverter(typeof(FormatoDate))]
        [JsonProperty(PropertyName = "fecha_creacion")]
        public DateTime fechaCreacion { get; set; }     

        public string calle { get; set; }
        
        public string esquina { get; set; }
        
        public string numero { get; set; }

        public string departamento { get; set; }

        public string sector { get; set; }

        public double longitud { get; set; }

        public double latitud { get; set; }

        public string descripcion { get; set; }

        [JsonProperty(PropertyName = "en_proceso")]
        public bool enProceso { get; set; }              
        
        public ICollection<DtoExtension> extensiones { get; set; }

        [JsonProperty(PropertyName = "origen_evento")]
        public DtoOrigenEvento origenEvento { get; set; }

        public virtual ICollection<DtoImagen> imagenes { get; set; }

        public virtual ICollection<DtoVideo> videos { get; set; }

        public virtual ICollection<DtoAudio> audios { get; set; }

        [JsonProperty(PropertyName = "id_zonas")]
        public ICollection<int> idZonas { get; set; }

        [JsonProperty(PropertyName = "id_departamento")]
        public int idDepartamento { get; set; }

        [JsonProperty(PropertyName = "id_sector")]
        public int idSector { get; set; }
    }
}
