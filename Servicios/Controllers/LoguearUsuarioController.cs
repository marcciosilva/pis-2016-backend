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
    public class LoguearUsuarioController : ApiController
    {
        [CustomAuthorizeAttribute()]
        [HttpPost]
        [LogFilter]
        [Route("users/login")]
        public DtoRespuesta ElegirRoles([FromBody] DtoRol rol)
        {
            try
            {
                string token = ObtenerToken.GetToken(Request);
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                IMetodos dbAL = new Metodos();
                dbAL.loguearUsuario(token, rol);
                return new DtoRespuesta(0,null);
            }
            catch (InvalidTokenException e)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                Emsys.Logs.Log.AgregarLogError("", "", "Emsys.ServiceLayer", "LoguearUsuarioController", 0, "Login", "Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorIniciarSesion);
                return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }
        }
    }
}
