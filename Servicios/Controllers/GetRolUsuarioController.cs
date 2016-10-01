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
    public class GetRolUsuarioController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("users/getroles")]
        public DtoRespuesta Get()
        {
            using (var context = new EmsysUserManager())
            {
                DtoRespuesta resp;
                try
                {
                    IMetodos dbAL = new Metodos();
                    DtoRol rol = dbAL.getRolUsuario(User.Identity.Name);
                    resp = new DtoRespuesta() { cod = 0, response = rol };
                }
                catch (Exception e)
                {
                    resp = new DtoRespuesta() { cod = 2, response = null };
                }                
                return resp;
            }            
        }
    }
}
