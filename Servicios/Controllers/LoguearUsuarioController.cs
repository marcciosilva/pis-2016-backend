using CapaAcessoDatos;
using DataTypeObject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Servicios.Controllers
{
    public class LoguearUsuarioController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("loguearUsuario")]
        public DtoRespuesta Get(string json)
        {
            DtoRespuesta resp;
            try
            {
                IMetodos dbAL = new Metodos();
                DtoRol rol = JsonConvert.DeserializeObject<DtoRol>(json);
                dbAL.loguearUsuario(User.Identity.Name, rol);
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
