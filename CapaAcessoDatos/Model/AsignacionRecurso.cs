namespace Emsys.DataAccesLayer.Model
{
    using DataTypeObject;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    [Table("AsignacionesRecursos")]
    public partial class AsignacionRecurso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Recurso Recurso { get; set; }

        public Extension_Evento Extension { get; set; }
        
        //public string  Descripcion { get; set; }

        public DateTime FechaArribo { get; set; }

        public bool ActualmenteAsignado { get; set; }

        public virtual List<AsignacionRecursoDescripcion> AsignacionRecursoDescripcion { get; set; }

    }
}
