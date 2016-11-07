using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataTypeObject
{
    public class DtoExtension
    {        
        public int id { get; set; }

        public DtoZona zona { get; set; }

        public string despachador { get; set; }

        [JsonProperty(PropertyName = "descripcion_despachadores")]
        public List<DtoDescripcion> descripcionDespachadores { get; set; }

        [JsonProperty(PropertyName = "descripcion_supervisor")]
        public string descripcionSupervisor { get; set; }

        [JsonProperty(PropertyName = "asignaciones_recursos")]
        public ICollection<DtoAsignacionRecurso> asignacionesRecursos { get; set; }

        public string estado { get; set; }

        [JsonConverter(typeof(FormatoDate))]
        [JsonProperty(PropertyName = "time_stamp")]
        public DateTime timeStamp { get; set; }

        [JsonProperty(PropertyName = "segunda_categoria")]
        public DtoCategoria segundaCategoria { get; set; }

        public ICollection<string> recursos { get; set; }

        public virtual ICollection<DtoImagen> imagenes { get; set; }

        public virtual ICollection<DtoVideo> videos { get; set; }

        public virtual ICollection<DtoAudio> audios { get; set; }

        [JsonProperty(PropertyName = "geo_ubicaciones")]
        public virtual ICollection<DtoGeoUbicacion> geoUbicaciones { get; set; }
    }
}
