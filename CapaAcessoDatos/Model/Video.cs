namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    

    [Table("Videos")]
    public partial class Video : Adjunto
    {
        public string Nombre { get; set; }

        public byte[] DatosVideo { get; set; }

    }
}
