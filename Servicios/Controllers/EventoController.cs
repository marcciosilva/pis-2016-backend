using Emsys.DataAccesLayer.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Servicios.Identity;

namespace Servicios.Controllers
{
    public class EventoController : ApiController
    {

        [ClaimsAuthorization(ClaimType = "FTE", ClaimValue = "1")]
        [HttpGet]
        [Route("eventos")]
        public string Get()
        {
            using (var context = new EmsysContext())
            {
                return "hola";//JsonConvert.SerializeObject(context.Evento.ToListAsync());
            }
        }
    }
}