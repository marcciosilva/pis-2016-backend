namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    [Table("Origen_Eventos")]
    public partial class Origen_Evento
    {
        [Key]
        public int Id { get; set; }

        public static Evento Evento { get; set; }

        public int IdOrigen { get; set; }

        public string TipoOrigen { get; set; }

        public DateTime TimeStamp { get; set; }

    }
}
