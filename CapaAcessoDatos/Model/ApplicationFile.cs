namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    [Table("ApplicationFiles")]
    public partial class ApplicationFile
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public byte[] FileData { get; set; }
    }
}
