namespace Emsys.DataAccesLayer.Model
{
    using DataTypeObject;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public enum EstadoRecurso
    {
        // Si no ha sido tomado por ningun usuario.
        Disponible,
        // Si ha sido tomado por ningun usuario.
        NoDisponible
    }

    public enum EstadoAsignacionRecurso
    {
        // Si es posible asignarlo a un evento.
        Libre,
        // Si ha sido asignado al maximo de eventos posible.
        Operativo
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
        public EstadoRecurso Estado { get; set; }

        [Required]
        public EstadoAsignacionRecurso EstadoAsignacion { get; set; }

        public virtual ICollection<Extension_Evento> Extensiones_Eventos { get; set; }

        public virtual ICollection<Grupo_Recurso> Grupos_Recursos { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
