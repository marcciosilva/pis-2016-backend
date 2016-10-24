namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("Videos")]
    public partial class Video
    {
        [Key]
        public int Id { get; set; }

        public virtual Usuario Usuario { get; set; }

        public DateTime FechaEnvio { get; set; }
        
        public virtual ApplicationFile VideoData { get; set; }

        public virtual Evento Evento { get; set; }

        public virtual Extension_Evento ExtensionEvento { get; set; }
    }
}
