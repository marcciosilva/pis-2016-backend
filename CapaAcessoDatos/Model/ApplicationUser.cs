namespace Emsys.DataAccesLayer.Model
{
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
        [MaxLength(20)]
        public string Nombre { get; set; }
        
        public Estado Estado { get; set; }    

        public virtual ICollection<Unidad_Ejecutora> Unidades_Ejecutoras { get; set; }

        public virtual ICollection<Log> Logs { get; set; }

        public virtual ICollection<Extension_Evento> Despachando { get; set; }

        public virtual ICollection<Evento> EventosCreados { get; set; }

        public virtual ICollection<Recurso> Recursos { get; set; }

    }
}
