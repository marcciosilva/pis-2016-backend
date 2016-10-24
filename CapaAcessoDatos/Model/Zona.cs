namespace Emsys.DataAccesLayer.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Zonas")]
    public partial class Zona
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]       
        public string Nombre { get; set; }
        
        [Required]
        public virtual Unidad_Ejecutora UnidadEjecutora { get; set; }

        public virtual ICollection<Extension_Evento> Extensiones_Evento { get; set; }

        public virtual ICollection<Sector> Sectores { get; set; }
    }
}
