using DataTypeObject;
using Servicios.Filtros;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Utils.Login;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;

namespace Servicios.Controllers
{
    public class CerrarSesionController : ApiController
    {
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
