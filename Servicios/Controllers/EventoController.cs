using Emsys.DataAccesLayer.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace Servicios.Controllers
{
    public class EventoController : ApiController
    {

        [Authorize]
        [HttpGet]
        [Route("eventos")]
        public string Get()
        {
            using (var context = new EmsysContext())
            {
                return JsonConvert.SerializeObject(context.Evento.ToListAsync());
            }
        }
    }
}