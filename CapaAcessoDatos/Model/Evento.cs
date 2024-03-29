﻿namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public enum EstadoEvento
    {
        Creado,
        Enviado
    }

    [Table("Eventos")]
    public partial class Evento
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string NombreInformante { get; set; }

        [MaxLength(50)]
        public string TelefonoEvento { get; set; }
        
        public virtual Categoria Categoria { get; set; }

        [Required]
        public EstadoEvento Estado { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }
        
        public virtual Usuario Usuario { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [MaxLength(150)]
        public string Calle { get; set; }

        [MaxLength(150)]
        public string Esquina { get; set; }

        [MaxLength(10)]
        public string Numero { get; set; }

        public Departamento Departamento { get; set; }
        
        public virtual Sector Sector { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }
        
        public string Descripcion { get; set; }

        [Required]
        public bool EnProceso { get; set; }

        public virtual ICollection<ExtensionEvento> ExtensionesEvento { get; set; }

        public virtual OrigenEvento OrigenEvento { get; set; }

        public virtual ICollection<Imagen> Imagenes { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        public virtual ICollection<Audio> Audios { get; set; }
    }
}
