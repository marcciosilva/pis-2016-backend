namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Zona")]
    public partial class Zona
    {
        [Key]
        public string NombreZona { get; set; }

        public virtual ICollection<Evento> Evento { get; set; }

    }
}
