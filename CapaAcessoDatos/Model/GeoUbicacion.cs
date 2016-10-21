namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    [Table("GeoUbicaciones")]
    public partial class GeoUbicacion
    {
        [Key]
        public int Id { get; set; }

        public virtual Usuario Usuario { get; set; }

        public DateTime FechaEnvio { get; set; }
        
        public double Longitud { get; set; }

        public double Latitud { get; set; }

        //public virtual Extension_Evento ExtensionEvento { get; set; }
    }
}
