namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Evento")]
    public partial class Evento
    {
        [Key]
        public string NombreGenerador { get; set; }

        public string Descripcion { get; set; }

        public string Direccion { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
