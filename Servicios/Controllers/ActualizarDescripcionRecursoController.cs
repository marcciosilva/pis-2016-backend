using DataTypeObject;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Servicios.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Servicios.Controllers
{
    public class ActualizarDescripcionRecursoController : ApiController
    {
        /// <summary>
        /// Servicio para actualizar la descripcion de un recurso.
        /// </summary>
        /// <param name="descParam">Entension y descripcion a agregar.</param>
        /// <returns>Respuesta definida en el documento de interfaz.</returns>
        [HttpPost]
        [Route("events/actualizarDescripcionRecurso")]
        [LogFilter]
        public DtoRespuesta ActualizarDescripcionRecurso([FromBody] DtoActualizarDescripcionParametro descParam)
        {
            IMetodos dbAL = new Metodos();
            try
            {
                string token = ObtenerToken.GetToken(Request);
                if (dbAL.ActualizarDescripcionRecurso(descParam, token))
                {
                    return new DtoRespuesta(0, new Mensaje(Mensajes.Correcto));
                }
                return new DtoRespuesta(500, "Ocurrio un error.");
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(1, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (InvalidExtensionException)
            {
                return new DtoRespuesta(Mensajes.IdentificadorExtensionIncorrecto, new Mensaje(Mensajes.GetDescription(Mensajes.IdentificadorExtensionIncorrecto)));
            }
            catch (InvalidExtensionForUserException)
            {
                return new DtoRespuesta(Mensajes.ExtensionNoAsignadaAlRecurso, new Mensaje(Mensajes.GetDescription(Mensajes.ExtensionNoAsignadaAlRecurso)));
            }
            catch (Exception e)
            {
                string token = ObtenerToken.GetToken(Request);
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "LoginController", 0, "Login", "Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, CodigosLog.ErrorIniciarSesionCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorIniciarSesion));
            }
        }
    }
}
