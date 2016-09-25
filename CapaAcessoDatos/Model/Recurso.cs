namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    public enum EstadosRecurso
    {
        // Si se encuentra con el maximo de eventos asignados
        Operativo,
        // Si ha sido tomado por un usuario y es posible asignarle mas eventos
        Disponible,
        // Si no ha sido tomado por ningun usuario
        NoDisponible
    }


    [Table("Recursos")]
    public partial class Recurso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Codigo { get; set; }

        [Required]
        public EstadosRecurso Estado { get; set; }

        public virtual ICollection<Extension_Evento> Extension_Evento { get; set; }
        
        public virtual ApplicationUser Usuario { get; set; }  
    }
}
