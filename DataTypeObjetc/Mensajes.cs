using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{

    public class Mensajes 
    {
        public const string Correcto = "Correcto";
        public const string UsuarioContraseñaInvalidos = "Usuario/Password inválido.";
        public const string TokenInvalido = "Token inválido.";
        public const string UsuarioLogueado = "Usuario ya autenticado.";
        public const string UsuarioNoAutenticado = "Usuario no autenticado.";
        public const string UsuarioTieneOperacionesNoFinalizadas = "El usuario tiene una operación no finalizada.";
        public const string ErrorCerraSesion = "Ocurrio un error en el servidor al cerrar la sesion";
        public const string ErrorIniciarSesion = "Ocurrio un error en el servidor al iniciar sesion";
        public const string ErrorGetRoles = "Ocurrio un error en el servidor al obtener roles.";
        public const string ServicioExternoNoDisponible = "El servicio externo no se encuentra disponible.";
        public const string RecursoNoDisponible = "El recurso seleccionado no se encuentra disponible.";
        public const string SeleccionZonasRecursosInvalida = "Seleccion invalida de zonas y recursos.";

        // Codigos de error.
        public const int ErrorIniciarSesionCod = 1001;
        public const int ErrorCerrarSesionCod = 2001;
        public const int ErrorConsumirServicioExternoCod = 3001;
        public const int LogAccionesCod = 9999;
        public const int LogLLamdosCod = 9998;
    }
}
