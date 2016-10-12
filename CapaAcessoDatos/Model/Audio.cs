﻿namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Audios")]
    public partial class Audio
    {
        [Key]
        public int Id { get; set; }

        public virtual Usuario Usuario { get; set; }

        public DateTime FechaEnvio { get; set; }

        public virtual ApplicationFile AudioData { get; set; }

        public virtual Evento Evento { get; set; }

        public virtual Extension_Evento ExtensionEvento { get; set; }
    }
}