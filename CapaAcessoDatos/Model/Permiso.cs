namespace Emsys.DataAccesLayer.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Permisos")]
    public partial class Permiso
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Nombre { get; set; }

        [MaxLength(150)]
        public string Descripcion { get; set; }

        [Required]
        [MaxLength(50)]
        public string Clave { get; set; }        

        public ICollection<Rol> Roles { get; set; }
    }
}
