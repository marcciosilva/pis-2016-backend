﻿namespace Emsys.DataAccesLayer.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
        
        public virtual ICollection<GrupoRecurso> GruposRecursos { get; set; }

        public virtual ICollection<AsignacionRecurso> AsignacionesRecurso { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
