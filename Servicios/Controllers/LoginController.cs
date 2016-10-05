using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using DataTypeObject;
using Utils.Login;
using DataTypeObjetc;
using Emsys.Logs;
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
            try
            {
                IMetodos dbAL = new Metodos();
                DtoAutenticacion autenticacion = dbAL.autenticarUsuario(user.username, user.password);
                return new DtoRespuesta(0, autenticacion);
            }
            catch (InvalidCredentialsException e)
            {
                return new DtoRespuesta(1, new Mensaje(Mensajes.UsuarioContraseñaInvalidos));
            }
            catch (Exception e)
            {
                Emsys.Logs.Log.AgregarLogError(user.username, "", "Emsys.ServiceLayer", "LoginController", 0, "Login", "Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorIniciarSesion);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorIniciarSesion));
            }          
        }
    }
}