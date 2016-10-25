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
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (Exception e)
            {
                string token = ObtenerToken.GetToken(Request);
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "ActualizarDescripcionRecursoController", 0, "ActualizarDescripcionRecurso", "Hubo un error al intentar actualizar la descripcion, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorActualizarDescripcionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorActualizarDescripcion));
            }
        }
    }
}
