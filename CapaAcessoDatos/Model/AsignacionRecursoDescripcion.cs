namespace Emsys.DataAccesLayer.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AsignacionRecursoDescripcion")]
    public partial class AsignacionRecursoDescripcion
    {
        public AsignacionRecursoDescripcion(string desc, DateTime fecha)
        {
            this.Descripcion = desc;
            this.Fecha = fecha;
            this.agregadaOffline = false;
        }

        public AsignacionRecursoDescripcion(string desc, DateTime fecha, bool offline)
        {
            this.Descripcion = desc;
            this.Fecha = fecha;
            this.agregadaOffline = offline;
        }

        public AsignacionRecursoDescripcion()
        {
        }

        [Key]
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public bool agregadaOffline { get; set; }

        public virtual AsignacionRecurso AsignacionRecurso { get; set; }
    }
}
