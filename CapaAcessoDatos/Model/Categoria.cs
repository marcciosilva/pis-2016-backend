namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


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

        [MaxLength(10)]
        public string Codigo { get; set; }

        [MaxLength(150)]
        public string Clave { get; set; }

        public NombrePrioridad Prioridad { get; set; }

        public bool Activo { get; set; }
    }
}
