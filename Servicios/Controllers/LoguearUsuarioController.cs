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
        [Authorize(Roles = "loguearUsuario")]
        [HttpGet]
        [Route("loguearUsuario")]
        public string Get(string json)
        {
            ICollection<DtoRol> roles = JsonConvert.DeserializeObject<ICollection<DtoRol>>(json);
            IMetodos dbAL = new Metodos();
            return JsonConvert.SerializeObject(dbAL.loguearUsuario(User.Identity.Name, roles));
        }
    }
}
