namespace Emsys.DataAccesLayer.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Grupos_Recursos")]
    public partial class Grupo_Recurso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }
        
        public virtual ICollection<Recurso> Recursos { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
