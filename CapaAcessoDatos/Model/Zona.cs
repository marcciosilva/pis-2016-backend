namespace Emsys.DataAccesLayer.Model
{
    using DataTypeObject;
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

        [Required]
        [MaxLength(200)]       
        public  string Nombre { get; set; }
        
        [Required]
        public virtual Unidad_Ejecutora Unidad_Ejecutora { get; set; }

        public virtual ICollection<Extension_Evento> Extensiones_Evento { get; set; }

        public virtual ICollection<Sector> Sectores { get; set; }

        public DtoZona getDto()
        {
            return new DtoZona() { id = Id, nombre = Nombre, nombre_ue = Unidad_Ejecutora.Nombre};            
        }          
    }
}
