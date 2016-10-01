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
        [Route("getRolUsuario")]
        public DtoRespuesta Get()
        {
            using (var context = new EmsysUserManager())
            {
                DtoRespuesta resp;
                try
                {
                    var user = context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    DtoRol rol = user.getRol();
                    resp = new DtoRespuesta() { Cod = 0, Response = rol };
                }
                catch (Exception e)
                {
                    resp = new DtoRespuesta() { Cod = 2, Response = null };
                }                
                return resp;
            }            
        }
    }
}
