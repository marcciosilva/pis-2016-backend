﻿namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Logs")]
    public partial class Log
    {
        [Key]
        public int Id { get; set; }
        
        public virtual string Usuario { get; set; }

        public DateTime TimeStamp { get; set; }

        [MaxLength(50)]
        public string Terminal { get; set; }

        [MaxLength(50)]
        public string Modulo { get; set; }

        [MaxLength(50)]
        public string Entidad { get; set; }

        public int idEntidad { get; set; }

        [MaxLength(50)]
        public string Accion { get; set; }

        public int Codigo { get; set; }

        public bool EsError { get; set; }

        public string Detalles { get; set; }
    }
}
