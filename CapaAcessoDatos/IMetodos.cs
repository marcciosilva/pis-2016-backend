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
        bool autenticarUsuario(string userName, string pass);

        bool registrarInicioUsuario(string userName, string token, DateTime fecha);

        ICollection<DtoEvento> listarEventos(string userName);

        DtoRol getRolUsuario(string userName);

        void loguearUsuario(string userName, DtoRol rol);

        void cerrarSesion(string userName);
    }
}
