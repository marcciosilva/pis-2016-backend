namespace Emsys.DataAccesLayer.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Sectores")]
    public partial class Sector
    {
        [Key]
        public int Id { get; set; }
        
        public virtual Zona Zona { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        
        public virtual ICollection<Evento> Eventos { get; set; }
    }
}
