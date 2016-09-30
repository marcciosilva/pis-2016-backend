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
        public string Get()
        {
            using (var context = new EmsysUserManager())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return JsonConvert.SerializeObject(user.getRol());
            }            
        }
    }
}
