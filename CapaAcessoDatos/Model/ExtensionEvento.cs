namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public enum EstadoExtension
    {
        FaltaDespachar,
        Despachado,
        Cerrado
    }

    [Table("Extensiones_Evento")]
    public partial class ExtensionEvento
    {
        [Key]
        public int Id { get; set; }
        
        public virtual Evento Evento { get; set; }
        
        public virtual Zona Zona { get; set; }

        public virtual Usuario Despachador { get; set; }

        public string DescripcionDespachador { get; set; }
        
        public int IdSupervisor { get; set; }

        public string DescripcionSupervisor { get; set; }

        [Required]
        public EstadoExtension Estado { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        public virtual Categoria SegundaCategoria { get; set; }
        
        public virtual ICollection<Imagen> Imagenes { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        public virtual ICollection<Audio> Audios { get; set; }

        public virtual ICollection<GeoUbicacion> GeoUbicaciones { get; set; }

        public virtual ICollection<AsignacionRecurso> AsignacionesRecursos { get; set; }
    }
}
