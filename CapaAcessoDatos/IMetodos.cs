using DataTypeObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAcessoDatos
{
    public interface IMetodos
    {
        ICollection<DtoEvento> listarEventos(string userName);

        bool loguearUsuario(string userName, DtoRol rol);

        bool cerrarSesion(string userName);
    }
}
