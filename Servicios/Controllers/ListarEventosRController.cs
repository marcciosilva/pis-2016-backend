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
        [Route("listar_eventos_r")]
        public string Get()
        {
            using (var context = new EmsysContext())
            {
                //List<DtEvento>
                //return JsonConvert.SerializeObject(context.Users.FirstOrDefault(u=> u.UserName==User.Identity.Name).Recursos.ToListAsync());
                return "";
            }
        }
    }
}
