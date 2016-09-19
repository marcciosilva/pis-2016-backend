namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Categoria")]
    public partial class Categoria
    {
        [Key]
        public string Codigo { get; set; }

        public string Clave { get; set; }

        public string Prioridad { get; set; }
        // La prioridad debe ser un enumerado
        public bool Activo { get; set; }

        public virtual ICollection<Evento> Evento { get; set; }
    }
}
