namespace Emsys.DataAccesLayer.Model
{
    using DataTypeObject;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public enum EstadoExtension
    {
        FaltaDespachar,
        Despachado,
        Cerrado
    }


    [Table("Extensiones_Evento")]
    public partial class Extension_Evento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Evento Evento { get; set; }

        [Required]
        public virtual Zona Zona { get; set; }

        public virtual ApplicationUser Despachador { get; set; }

        public string DescripcionDespachador { get; set; }
        
        public int IdSupervisor { get; set; }

        public string DescripcionSupervisor { get; set; }

        [Required]
        public EstadoExtension Estado { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        public virtual Categoria SegundaCategoria { get; set; }

        public virtual ICollection<Recurso> Recursos { get; set; }
              
        public DtoExtension getDto()        
        {
            List<string> recursos = new List<string>();
            foreach (Recurso r in Recursos)
            {
                recursos.Add(r.Codigo);
            }
            DtoCategoria cat = null;
            if (this.SegundaCategoria != null)
                cat = SegundaCategoria.getDto();

            return new DtoExtension()
            {
                zona = Zona.getDto(),
                descripcion_despachador = DescripcionDespachador,
                descripcion_supervisor= DescripcionSupervisor,
                estado = Estado.ToString().ToLower(),
                time_stamp = TimeStamp,
                categoria = cat,
                recursos= recursos
            };
        }        
    }
}
