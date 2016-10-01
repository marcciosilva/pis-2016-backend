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
using DataTypeObject;
using Emsys.DataAccesLayer.Model;
using Servicios.Filtros;

namespace Servicios.Controllers
{
    public class EventoController : ApiController
    {       
        //[HttpGet]
        //[Route("eventos")]
        //public string Get()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        /*
        //        ICollection<DtoZona> zonas = new List<DtoZona>();
        //        foreach (Zona z in context.Zonas)
        //        {
        //            zonas.Add(z.getDto());
        //        }
        //        ICollection<DtoRecurso> recursos = new List<DtoRecurso>();

        //        DtoRol rol = new DtoRol() { Zonas = zonas, Recursos = recursos };
        //        return JsonConvert.SerializeObject(rol);*/
        //        return context.Users.FirstOrDefault().Zonas.Count().ToString();
        //    }
        //}


        [CustomAuthorizeAttribute("Permiso","a")]
        [HttpGet]
        [Route("eventos")]
        public async Task<IHttpActionResult> Get()
        {
            using (var context = new EmsysContext())
            {
                return Ok(await context.Evento.ToListAsync());
            }
        }
    }
}