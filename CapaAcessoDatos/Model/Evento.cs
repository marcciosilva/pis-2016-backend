namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public enum EstadoEvento
    {
        Creado,
        Enviado
    }


    [Table("Evento")]
    public partial class Evento
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string NombreInformante { get; set; }

        public virtual Categoria Categoria { get; set; }

        public EstadoEvento Estado { get; set; }

        public DateTime TimeStamp { get; set; }

        public virtual ApplicationUser Usuario { get; set; }

        public DateTime FechaCreacion { get; set; }

        [MaxLength(150)]
        public string Calle { get; set; }

        [MaxLength(150)]
        public string Esquina { get; set; }

        [MaxLength(10)]
        public string Numero { get; set; }

        public virtual Sector Sector { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }
        
        public string Descripcion { get; set; }

        public bool EnProceso { get; set; }

        public virtual ICollection<Extension_Evento> ExtensionEvento { get; set; }

        public virtual Origen_Evento Origen_Evento { get; set; }
                        
    }
}
