namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    [Table("ApplicationRoles")]
    public partial class Rol
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }
        
        public virtual ICollection<Permiso> Permisos { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
