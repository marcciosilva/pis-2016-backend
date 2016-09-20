namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("Unidades_Ejecutoras")]
    public partial class Unidad_Ejecutora
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
       
        public  string Nombre { get; set; }

        public virtual ICollection<Zona> Zona { get; set; }

        public virtual ICollection<Usuario_UE> Usuarios_UE { get; set; }
    }
}
