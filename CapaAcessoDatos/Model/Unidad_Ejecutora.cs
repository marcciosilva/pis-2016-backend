namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("Unidades_Ejecutoras")]
    public partial class Unidad_Ejecutora
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Nombre { get; set; }

        public virtual ICollection<Zona> Zona { get; set; }

        public virtual ICollection<ApplicationUser> Usuarios { get; set; }

        public virtual ICollection<Unidad_Ejecutora> Unidades_Ejecutoras { get; set; }
    }
}
