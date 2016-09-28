namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    [Table("Permisos")]
    public partial class Permiso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Clave { get; set; }

        [MaxLength(150)]
        public string Descripcion { get; set; }

        public ICollection<ApplicationRole> Roles { get; set; }
        
    }
}
