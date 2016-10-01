using CapaAcessoDatos;
using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Newtonsoft.Json;
using Servicios.Filtros;
using Servicios.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utils.Login;

namespace Servicios.Controllers
{
    public class ListarEventosController : ApiController
    {
        [CustomAuthorizeAttribute("listarEventos")]
        [HttpGet]
        [Route("events")]
        public DtoRespuesta Get()
        {
            DtoRespuesta resp;
            try
            {
                IMetodos dbAL = new Metodos();
                ICollection<DtoEvento> eventos = dbAL.listarEventos(ObtenerUsuario.ObtenerNombreUsuario(Request));
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
