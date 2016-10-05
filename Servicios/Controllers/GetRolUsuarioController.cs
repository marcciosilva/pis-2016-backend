using DataTypeObject;
using DataTypeObjetc;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Newtonsoft.Json;
using Servicios.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utils.Login;
namespace Servicios.Controllers
{
    public class GetRolUsuarioController : ApiController
    {
        [CustomAuthorizeAttribute()]
        [HttpPost]
        [LogFilter]
        [Route("users/getroles")]
        public DtoRespuesta GetRoles()
        {
            try
            {
                string token = ObtenerToken.GetToken(Request);
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }

                IMetodos dbAL = new Metodos();                
                DtoRol rol = dbAL.getRolUsuario(token);
                return new DtoRespuesta(0, rol);
                           
            }
            catch (InvalidTokenException e)
            {
                Emsys.Logs.Log.AgregarLogError("", "", "Emsys.ServiceLayer", "GetRolUsuarioController", 0, "GetRoles", "Hubo un error al intentar obtener roles de un usuario, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorCerrarSesion);
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                Emsys.Logs.Log.AgregarLogError("", "", "Emsys.ServiceLayer", "GetRolUsuarioController", 0, "GetRoles", "Hubo un error al intentar obtener roles de un usuario, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorIniciarSesion);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorGetRoles));
            }         
        }
    }
}
