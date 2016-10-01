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
                resp = new DtoRespuesta() { Cod = 0, Response = null };
            }
            catch (Exception e)
            {
                resp = new DtoRespuesta() { Cod = 2, Response = null };
            }
            return resp;
        }
    }
}
