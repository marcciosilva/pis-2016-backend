namespace Emsys.DataAccesLayer.Model
{
    using DataTypeObject;
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

        [Required]
        public virtual Categoria Categoria { get; set; }

        [Required]
        public EstadoEvento Estado { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public virtual ApplicationUser Usuario { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [MaxLength(150)]
        public string Calle { get; set; }

        [MaxLength(150)]
        public string Esquina { get; set; }

        [MaxLength(10)]
        public string Numero { get; set; }

        public Departamento Departamento { get; set; }

        [Required]
        public virtual Sector Sector { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }
        
        public string Descripcion { get; set; }

        [Required]
        public bool EnProceso { get; set; }

        public virtual ICollection<Extension_Evento> ExtensionesEvento { get; set; }

        public virtual Origen_Evento Origen_Evento { get; set; }

        public DtoEvento getEvento()
        {
            List<DtoExtension> extensiones = new List<DtoExtension>();
            foreach (Extension_Evento e in ExtensionesEvento)
            {
                extensiones.Add(e.getDto());
            }
            return new DtoEvento()
            {
                Id = this.Id,
                NombreInformante = this.NombreInformante,
                Categoria = this.Categoria.getDto(),
                Estado = (DataTypeObject.EstadoEvento)Array.IndexOf(Enum.GetValues(Estado.GetType()), Estado),
                TimeStamp = this.TimeStamp,
                FechaCreacion = this.FechaCreacion,
                Departamento = this.Departamento.Nombre,
                Calle = this.Calle,
                Esquina = this.Esquina,
                Numero = this.Numero,
                Sector = this.Sector.Nombre,
                Latitud = this.Latitud,
                Longitud = this.Longitud,
                Descripcion = this.Descripcion,
                EnProceso = this.EnProceso,
                ExtensionesEvento = extensiones
            };
        }                        
    }
}
