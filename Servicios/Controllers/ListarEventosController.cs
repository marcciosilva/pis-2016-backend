using CapaAcessoDatos;
using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Newtonsoft.Json;
using Servicios.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Servicios.Controllers
{
    public class ListarEventosController : ApiController
    {
        [Authorize(Roles = "listarEventos")]
        [HttpGet]
        [Route("listarEventos")]
        public DtoRespuesta Get()
        {
            DtoRespuesta resp;
            try
            {
                IMetodos dbAL = new Metodos();
                ICollection<DtoEvento> eventos = dbAL.listarEventos(User.Identity.Name);
                resp = new DtoRespuesta() { cod = 0, response = eventos };
            }
            catch (Exception e)
            {
                resp = new DtoRespuesta() { cod = 2, response = null };
            }
            Console.WriteLine(JsonConvert.SerializeObject(resp));
            return resp;            
        }
    }
}
