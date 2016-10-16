﻿namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    [Table("Imagenes")]
    public partial class Imagen
    {
        [Key]
        public int Id { get; set; }

        public virtual Usuario Usuario { get; set; }

        public DateTime FechaEnvio { get; set; }
        
        public virtual ApplicationFile ImagenData { get; set; }

        public virtual Evento Evento { get; set; }

        public virtual Extension_Evento ExtensionEvento { get; set; }
    }
}
