using Emsys.DataAccesLayer.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using DataTypeObject;
using Emsys.DataAccesLayer.Model;
using Utils.Login;
using DataTypeObjetc;
using Emsys.Logs;
using CapaAcessoDatos;
using Servicios.Filtros;

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
                if (dbAL.autenticarUsuario(user.username, Contraseñas.GetSHA1(user.password)))
                {
                    var token = TokenGenerator.ObetenerToken();

                    if (dbAL.registrarInicioUsuario(user.username, token, DateTime.Now))
                    {
                        return new DtoRespuesta(0, new Authenticate(token, null));
                    }
                    return new DtoRespuesta(1, new Mensaje(Mensajes.ErrorIniciarSesion));
                }
                return new DtoRespuesta(1, new Mensaje(Mensajes.UsuarioContraseñaInvalidos));
            }
            catch (Exception e)
            {
                Emsys.Logs.Log.AgregarLogError(user.username, "", "Emsys.ServiceLayer", "LoginController", 0, "Login", "Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorIniciarSesion);
                return new DtoRespuesta(1, new Mensaje(Mensajes.ErrorIniciarSesion));
            }          
        }
    }
}