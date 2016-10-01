using CapaAcessoDatos;
using DataTypeObject;
using DataTypeObjetc;
using Emsys.DataAccesLayer.Core;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Utils.Login;

namespace Servicios.Controllers
{
    public class CerrarSesionController : ApiController
    {
        [HttpGet]
        [Route("users/logout")]
        public GenericResponse CerrarSesion()
        {
            using (var db = new EmsysContext())
            {
                try
                {
                    IMetodos dbAL = new Metodos();
                    dbAL.cerrarSesion(ObtenerUsuario.ObtenerNombreUsuario(Request));
                    var user = db.Users.Where(x => x.UserName == ObtenerUsuario.ObtenerNombreUsuario(Request)).FirstOrDefault();
                    if (user!= null)
                    {
                        var usuarioOperacionesNoFinalizadas = false;//TODO esto no se puede hacer hasta tener operaciones
                        if (usuarioOperacionesNoFinalizadas) {
                            return new GenericResponse(Mensajes.GetCodMenssage(Mensajes.UsuarioTieneOperacionesNoFinalizadas),
                                new Logout(Mensajes.UsuarioTieneOperacionesNoFinalizadas));
                        }
                        user.Token = null;
                        user.FechaInicioSesion = null;
                        return new GenericResponse(Mensajes.GetCodMenssage(Mensajes.Correcto), new Logout(null));
                    }
                    return new GenericResponse(Mensajes.GetCodMenssage(Mensajes.UsuarioNoAutenticado), new Logout(Mensajes.UsuarioNoAutenticado));
                }
                catch (Exception e)
                {
                    Emsys.Logs.Log.AgregarLogError(ObtenerUsuario.ObtenerNombreUsuario(Request), "", "Emsys.ServiceLayer", "CerrarSesionController", 0, "Logout", "Hubo un error al intentar cerrar sesion, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorCerrarSesion);
                    return new GenericResponse(Mensajes.GetCodMenssage(Mensajes.ErrorCerraSesion), new Logout(Mensajes.ErrorCerraSesion));
                }
            }            
        }
    }
}
