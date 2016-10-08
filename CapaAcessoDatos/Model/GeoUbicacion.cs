namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    

    [Table("GeoUbicaciones")]
    public partial class GeoUbicacion : Adjunto
    {
        public double Longitud { get; set; }

        public double Latitud { get; set; }

    }
}
