namespace Emsys.DataAccesLayer.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public enum NombrePrioridad
    {
        Alta,
        Media,
        Baja
    }

    [Table("Categorias")]
    public partial class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(150)]
        public string Clave { get; set; }

        [Required]
        public NombrePrioridad Prioridad { get; set; }

        [Required]
        public bool Activo { get; set; }

        public virtual ICollection<ExtensionEvento> ExtensionesEvento { get; set; }

        public virtual ICollection<Evento> Eventos { get; set; }
    }
}
