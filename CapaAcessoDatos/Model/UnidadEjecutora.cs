namespace Emsys.DataAccesLayer.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Unidades_Ejecutoras")]
    public partial class UnidadEjecutora
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Nombre { get; set; }

        public virtual ICollection<Zona> Zonas { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
