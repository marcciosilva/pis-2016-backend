﻿namespace Emsys.DataAccesLayer.Model
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

        public string TelefonoEvento { get; set; }

        [Required]
        public virtual Categoria Categoria { get; set; }

        [Required]
        public EstadoEvento Estado { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }
        
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

        public DtoEvento getDto()
        {
            List<DtoExtension> extensiones = new List<DtoExtension>();
            foreach (Extension_Evento e in ExtensionesEvento)
            {
                extensiones.Add(e.getDto());
            }
            string dep = null;
            string sec = null;
            if (this.Departamento != null)
                dep = this.Departamento.Nombre;
            if (this.Sector != null)
                sec = this.Sector.Nombre;

            return new DtoEvento()
            {            
                id = this.Id,
                informante = this.NombreInformante,
                telefono = TelefonoEvento,
                categoria = this.Categoria.getDto(),
                estado = Estado.ToString().ToLower(),
                time_stamp = this.TimeStamp,
                fecha_creacion = this.FechaCreacion,                
                departamento = dep,
                calle = this.Calle,
                esquina = this.Esquina,
                numero = this.Numero,
                sector = sec,
                latitud = this.Latitud,
                longitud = this.Longitud,
                descripcion = this.Descripcion,
                en_proceso = this.EnProceso,
                extensiones = extensiones
            };
        }                        
    }
}
