namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    

    [Table("Audios")]
    public partial class Audio : Adjunto
    {
        public string Nombre { get; set; }

        public string DatosAudioString { get; set; }

    }
}
