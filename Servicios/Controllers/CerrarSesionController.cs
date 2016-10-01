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
        public DtoRespuesta Get()
        {
            using (var context = new EmsysUserManager())
            {
                DtoRespuesta resp;
                try
                {
                    IMetodos dbAL = new Metodos();
                    dbAL.cerrarSesion(User.Identity.Name);
                    resp = new DtoRespuesta() { cod = 0, response = null };
                    if (Request.GetOwinContext() != null)
                    {
                        Request.GetOwinContext().Authentication.SignOut();
                    }
                }
                catch (Exception e)
                {
                    resp = new DtoRespuesta() { cod = 1, response = null };
                }
                return resp;
            }            
        }
    }
}
