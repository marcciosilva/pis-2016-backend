using CapaAcessoDatos;
using DataTypeObject;
using DataTypeObjetc;
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
        [HttpPost]
        [Route("users/getroles")]
        public DtoRespuesta GetRoles()
        {
            try
            {  
                IMetodos dbAL = new Metodos();
                var nombreUsuario = ObtenerUsuario.ObtenerNombreUsuario(Request);
                if (nombreUsuario != null)
                {
                    DtoRol rol = dbAL.getRolUsuario(nombreUsuario);
                    return new DtoRespuesta(0, rol);
                }
                else
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }                
            }
            catch (Exception e)
            {
                Emsys.Logs.Log.AgregarLogError(ObtenerUsuario.ObtenerNombreUsuario(Request), "", "Emsys.ServiceLayer", "GetRolUsuarioController", 0, "GetRoles", "Hubo un error al intentar obtener roles de un usuario, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorIniciarSesion);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorGetRoles));
            }         
        }
    }
}
