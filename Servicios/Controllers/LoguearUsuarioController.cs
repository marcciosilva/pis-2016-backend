using CapaAcessoDatos;
using DataTypeObject;
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
        [HttpGet]
        [Route("users/login")]
        public DtoRespuesta Get(string json)
        {
            DtoRespuesta resp;
            try
            {
                IMetodos dbAL = new Metodos();
                DtoRol rol = JsonConvert.DeserializeObject<DtoRol>(json);
                dbAL.loguearUsuario(ObtenerUsuario.ObtenerNombreUsuario(Request), rol);
                resp = new DtoRespuesta() { cod = 0, response = null };
            }
            catch (Exception e)
            {
                resp = new DtoRespuesta() { cod = 2, response = null };
            }
            return resp;
        }
    }
}
