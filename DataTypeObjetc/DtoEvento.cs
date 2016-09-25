using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObjetc
{
    public enum EstadoEvento
    {
        Creado,
        Enviado
    }

    public class DtoEvento
    {
        public int Id { get; set; }
        
        public string NombreInformante { get; set; }

        public virtual DtoCategoria Categoria { get; set; }

        public EstadoEvento Estado { get; set; }

        public DateTime TimeStamp { get; set; }
        
        public DateTime FechaCreacion { get; set; }
        
        public string Departamento { get; set; }

        public string Calle { get; set; }
        
        public string Esquina { get; set; }
        
        public string Numero { get; set; }

        public string Sector { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public string Descripcion { get; set; }

        public bool EnProceso { get; set; }

        public ICollection<DtoExtension> ExtensionesEvento { get; set; }
                
    }
}
