namespace Emsys.DataAccesLayer.Model
{
    using DataTypeObject;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    public enum Estado
    {
        Activo,
        Inactivo
    }
        
    public class ApplicationUser : IdentityUser
    {

        public string NombreUsuario { get; set; }

        public string Contraseña { get; set; }

        public string  Token { get; set; }

        public DateTime? FechaInicioSesion { get; set; }

        [MaxLength(20)]
        public string Nombre { get; set; }


        [Required]
        public Estado Estado { get; set; }    

        public virtual ICollection<Unidad_Ejecutora> Unidades_Ejecutoras { get; set; }

        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<Zona> Zonas { get; set; }

        public virtual ICollection<Extension_Evento> Despachando { get; set; }

        public virtual ICollection<Evento> EventosCreados { get; set; }

        public virtual ICollection<Grupo_Recurso> Grupos_Recursos { get; set; }

        public virtual ICollection<Recurso> Recurso { get; set; }

        public virtual ICollection<ApplicationRole> ApplicationRoles { get; set; }

    }
}
