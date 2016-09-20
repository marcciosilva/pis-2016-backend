namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    [Table("Departamentos")]
    public partial class Departamento
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Nombre { get; set; }

        public virtual ICollection<Evento> Eventos { get; set; }
    }
}
