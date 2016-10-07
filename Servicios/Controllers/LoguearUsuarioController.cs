using DataTypeObject;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Servicios.Filtros;
using System;
using System.Web.Http;
using Utils.Login;

namespace Servicios.Controllers
{
    public class LoguearUsuarioController : ApiController
    {
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
    }
}
