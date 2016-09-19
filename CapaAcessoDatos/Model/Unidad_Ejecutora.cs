namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("Unidad_Ejecutora")]
    public partial class Unidad_Ejecutora
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
       
        public  string Nombre { get; set; }

        public static ICollection<Zona> Zona { get; set; }



                
    }
}
