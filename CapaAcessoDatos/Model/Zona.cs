namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("Zonas")]
    public partial class Zona
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]       
        public  string Nombre { get; set; }

        public virtual ICollection<Extension_Evento> Extensiones_Evento { get; set; }

        [Required]
        public virtual Unidad_Ejecutora Unidad_Ejecutora { get; set; }

        public virtual ICollection<Sector> Sectores { get; set; }                
    }
}
