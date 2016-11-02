namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Origen_Eventos")]
    public partial class OrigenEvento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Evento Evento { get; set; }

        public int IdOrigen { get; set; }

        [Required]
        public string TipoOrigen { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
