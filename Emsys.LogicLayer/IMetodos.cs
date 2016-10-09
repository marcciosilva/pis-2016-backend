using DataTypeObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer
{
    public interface IMetodos
    {

        DtoAutenticacion autenticarUsuario(string userName, string password);

        DtoRol getRolUsuario(string token);

        bool autorizarUsuario(string token, string[] etiquetas);

        bool loguearUsuario(string token, DtoRol rol);

        ICollection<DtoEvento> listarEventos(string token);

        bool cerrarSesion(string token);

        string getNombreUsuario(string token);

        void AgregarLog(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo);

        void AgregarLogError(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo);
        
        DtoEvento verInfoEvento(string token, int idEvento);

        bool adjuntarGeoUbicacion(string token, DtoGeoUbicacion ubicacion);
    }
}
