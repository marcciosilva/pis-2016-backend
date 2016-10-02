using CapaAcessoDatos;
using DataTypeObject;
using Emsys.DataAccesLayer.Core;
using Newtonsoft.Json;
using Servicios.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utils.Login;
namespace Servicios.Controllers
{
    public class GetRolUsuarioController : ApiController
    {
        [CustomAuthorizeAttribute()]
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
                    DtoRol rol = dbAL.getRolUsuario(ObtenerUsuario.ObtenerNombreUsuario(Request));
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
