using CapaAcessoDatos;
using DataTypeObject;
using DataTypeObjetc;
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
        [Route("users/login")]
        public DtoRespuesta ElegirRole(string json)
        {
            try
            {
                IMetodos dbAL = new Metodos();
                DtoRol rol = JsonConvert.DeserializeObject<DtoRol>(json);
                dbAL.loguearUsuario(ObtenerUsuario.ObtenerNombreUsuario(Request), rol);
                return new DtoRespuesta(0,null);
            }
            catch (Exception e)
            {
                Emsys.Logs.Log.AgregarLogError(ObtenerUsuario.ObtenerNombreUsuario(Request), "", "Emsys.ServiceLayer", "LoguearUsuarioController", 0, "Login", "Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorIniciarSesion);
                return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }
        }
    }
}
