namespace Emsys.DataAccesLayer.Model
{
    using DataTypeObject;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    [Table("AsignacionRecursoDescripcion")]
    public partial class AsignacionRecursoDescripcion
    {
        public AsignacionRecursoDescripcion(string desc, DateTime fecha)
        {
            Descripcion = desc;
            Fecha = fecha;
        }
        public AsignacionRecursoDescripcion()
        {

        }
        [Key]
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public virtual AsignacionRecurso AsignacionRecurso { get; set; }
    }
}
