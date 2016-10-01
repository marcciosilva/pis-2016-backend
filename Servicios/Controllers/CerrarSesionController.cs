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
    public class CerrarSesionController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("cerrarSesion")]
        public string Get()
        {
            using (var context = new EmsysUserManager())
            {
                IMetodos dbAL = new Metodos();
                if (Request.GetOwinContext() != null)
                {
                    Request.GetOwinContext().Authentication.SignOut();
                }
                return JsonConvert.SerializeObject(dbAL.cerrarSesion(User.Identity.Name));
            }            
        }
    }
}
