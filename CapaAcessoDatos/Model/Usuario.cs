namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public enum Estado
    {
        Activo,
        Inactivo
    }
        
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public string NombreLogin { get; set; }

        public string Contraseña { get; set; }

        public string Token { get; set; }

        public DateTime? UltimoSignal { get; set; }

        public DateTime? FechaInicioSesion { get; set; }

        [MaxLength(200)]
        public string Nombre { get; set; }

        [Required]
        public Estado Estado { get; set; }

        public string RegistrationTokenFirebase { get; set; }

        public virtual ICollection<UnidadEjecutora> UnidadesEjecutoras { get; set; }

        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<Zona> Zonas { get; set; }

        public virtual ICollection<ExtensionEvento> Despachando { get; set; }

        public virtual ICollection<Evento> EventosCreados { get; set; }

        public virtual ICollection<GrupoRecurso> GruposRecursos { get; set; }

        public virtual ICollection<Recurso> Recurso { get; set; }

        public virtual ICollection<Rol> ApplicationRoles { get; set; }
    }
}
