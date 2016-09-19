namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("Zona")]
    public partial class Zona
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
       
        public  string Nombre { get; set; }

        public static Extension_Evento Extension_Evento { get; set; }

        public static Unidad_Ejecutora Unidad_Ejecutora { get; set; }


                
    }
}
