using CapaAcessoDatos;
using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Servicios.Controllers
{
    public class ListarEventosRController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("listarEventos")]
        public string Get()
        {
            IMetodos dbAL = new Metodos();
            return JsonConvert.SerializeObject(dbAL.listarEventos(User.Identity.Name));
            
        }
    }
}
