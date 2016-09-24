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

        public virtual Evento Evento { get; set; }

        public virtual Zona Zona { get; set; }

        public virtual ApplicationUser Despachador { get; set; }

        public string DescripcionDespachador { get; set; }
        
        public int IdSupervisor { get; set; }

        public string DescripcionSupervisor { get; set; }

        public EstadoExtension Estado { get; set; }

        public DateTime TimeStamp { get; set; }

        public virtual Categoria SegundaCategoria { get; set; }

        public virtual ICollection<Recurso> Recursos { get; set; }
        
    }
}
