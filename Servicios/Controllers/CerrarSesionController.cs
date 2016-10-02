using CapaAcessoDatos;
using DataTypeObject;
using DataTypeObjetc;
using Emsys.DataAccesLayer.Core;
using Servicios.Filtros;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Utils.Login;

namespace Servicios.Controllers
{
    public class CerrarSesionController : ApiController
    {
        [CustomAuthorizeAttribute()]
        [HttpGet]
        [Route("users/logout")]
        public DtoRespuesta CerrarSesion()
        {            
            try
            {
                string nombreUsuario = ObtenerUsuario.ObtenerNombreUsuario(Request);
                // TODO esto no se puede hacer hasta tener operaciones.
                var usuarioOperacionesNoFinalizadas = false;
                if (usuarioOperacionesNoFinalizadas)
                {
                    return new DtoRespuesta(5, new Mensaje(Mensajes.UsuarioTieneOperacionesNoFinalizadas));
                }
                IMetodos dbAL = new Metodos();                        
                dbAL.cerrarSesion(nombreUsuario);
                return new DtoRespuesta(0, null);
            }
            catch (Exception e)
            {
                Emsys.Logs.Log.AgregarLogError(ObtenerUsuario.ObtenerNombreUsuario(Request), "", "Emsys.ServiceLayer", "CerrarSesionController", 0, "Logout", "Hubo un error al intentar cerrar sesion, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorCerrarSesion);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorCerraSesion));
            }          
        }
    }
}
