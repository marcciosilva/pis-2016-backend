namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    
    public partial class Adjunto
    {
        [Key]
        public int Id { get; set; }
        
        public virtual ApplicationUser Usuario {get; set; }
       
        public DateTime FechaEnvio { get; set; }
        
        public string Descripcion { get; set; }
        
    }
}
