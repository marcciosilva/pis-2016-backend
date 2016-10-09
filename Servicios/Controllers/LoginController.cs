using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DataTypeObject;
using Servicios.Filtros;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;

namespace Servicios.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("users/authenticate")]
        [LogFilter]
        public async Task<DtoRespuesta> Login([FromBody] DtoUser user)
        {
            IMetodos dbAL = new Metodos();
            try
            {
                DtoAutenticacion autenticacion = dbAL.autenticarUsuario(user.username, user.password);
                return new DtoRespuesta(0, autenticacion);
            }
            catch (SesionActivaException e)
            {
                return new DtoRespuesta(1, new Mensaje(Mensajes.SesionActiva));
            }
            catch (InvalidCredentialsException e)
            {
                return new DtoRespuesta(1, new Mensaje(Mensajes.UsuarioContraseñaInvalidos));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(user.username, "", "Emsys.ServiceLayer", "LoginController", 0, "Login", "Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorIniciarSesion));
            }          
        }


        [CustomAuthorizeAttribute()]
        [HttpPost]
        [LogFilter]
        [Route("users/getroles")]
        public DtoRespuesta GetRoles()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                DtoRol rol = dbAL.getRolUsuario(token);
                return new DtoRespuesta(0, rol);

            }
            catch (InvalidTokenException e)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "GetRolUsuarioController", 0, "GetRoles", "Hubo un error al intentar obtener roles de un usuario, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorGetRoles));
            }
        }


        [CustomAuthorizeAttribute()]
        [HttpPost]
        [LogFilter]
        [Route("users/login")]
        public DtoRespuesta ElegirRoles([FromBody] DtoRol rol)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                if (dbAL.loguearUsuario(token, rol))
                {
                    return new DtoRespuesta(0, null);
                }
                else
                {
                    return new DtoRespuesta(3, new Mensaje(Mensajes.SeleccionZonasRecursosInvalida));
                }
            }
            catch (RecursoNoDisponibleException e)
            {
                return new DtoRespuesta(4, new Mensaje(Mensajes.RecursoNoDisponible));
            }
            catch (InvalidTokenException e)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "LoguearUsuarioController", 0, "Login", "Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }
        }


        [CustomAuthorizeAttribute()]
        [HttpPost]
        [LogFilter]
        [Route("users/logout")]
        public DtoRespuesta CerrarSesion()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                // TODO esto no se puede hacer hasta tener operaciones.
                var usuarioOperacionesNoFinalizadas = false;
                if (usuarioOperacionesNoFinalizadas)
                {
                    return new DtoRespuesta(5, new Mensaje(Mensajes.UsuarioTieneOperacionesNoFinalizadas));
                }
                dbAL.cerrarSesion(token);
                return new DtoRespuesta(0, null);
            }
            catch (InvalidTokenException e)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "CerrarSesionController", 0, "Logout", "Hubo un error al intentar cerrar sesion, se adjunta excepcion: " + e.Message, Mensajes.ErrorCerrarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorCerraSesion));
            }
        }
    }
}