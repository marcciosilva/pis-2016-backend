namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public enum EstadoExtension
    {
        FaltaDespachar,
        Despachado,
        Cerrado
    }


    [Table("Extensiones_Evento")]
    public partial class Extension_Evento
    {
        [Key]
        public int Id { get; set; }

        public static Evento Evento { get; set; }

        public static Zona Zona { get; set; }

        //public static Usuario Despachador { get; set; }

        public string DescripcionDespachador { get; set; }
        
        public int IdSupervisor { get; set; }

        public string DescripcionSupervisor { get; set; }

        public EstadoExtension Estado { get; set; }

        public DateTime TimeStamp { get; set; }

        public static Categoria SegundaCategoria { get; set; }
        
    }
}
