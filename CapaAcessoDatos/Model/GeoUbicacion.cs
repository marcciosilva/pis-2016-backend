namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("GeoUbicaciones")]
    public partial class GeoUbicacion
    {
        [Key]
        public int Id { get; set; }

        public virtual Usuario Usuario { get; set; }

        public DateTime FechaEnvio { get; set; }
        
        public double Longitud { get; set; }

        public double Latitud { get; set; }

        public virtual ExtensionEvento Extension { get; set; }
    }
}
