namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    [Table("Usuarios_UE")]
    public partial class Usuario_UE
    {
        [Key]
        public int Id { get; set; }

        //public Zona Zona {get; set; }

        //public Usuario Usuario { get; set; }

        //public Unidad_Ejecutora Unidad_Ejecutora { get; set; }

    }
}
