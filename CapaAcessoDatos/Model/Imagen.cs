namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    

    [Table("Imagenes")]
    public partial class Imagen : Adjunto
    {
        public string Nombre { get; set; }

        public string DatosImagenString { get; set; }

    }
}
