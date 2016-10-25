using DataTypeObject;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Newtonsoft.Json;
using Servicios.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Servicios.Controllers
{
    public class EventosController : ApiController
    {
        /// <summary>
        /// Servicio que retorna los eventos a partir de un token enviado en el header.
        /// </summary>
        /// <returns>Retorna un objeto definido en el documento de interfaz.</returns>
        [CustomAuthorizeAttribute("listarEventos")]
        [HttpGet]
        [LogFilter]
        [Route("eventos/listar")]
        public DtoRespuesta ListarEventos()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {                
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }
                                
                ICollection<DtoEvento> lista = dbAL.listarEventos(token);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, lista);
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "ListarEventos", "Hubo un error al intentar listar eventos de un usuario, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorListarEventosCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorListarEventos));
            }
        }

        /// <summary>
        /// Servicio que dado un evento se obtiene la informacion del mismo.
        /// </summary>
        /// <param name="idEvento">Identificador del evento.</param>
        /// <returns>Devuelve la informacion del evento siguiendo el documento de interfaz.</returns>
        [CustomAuthorizeAttribute()]
        [HttpGet]
        [LogFilter]
        [Route("eventos/obtener")]
        public DtoRespuesta getEvento(int idEvento)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                DtoEvento evento = dbAL.verInfoEvento(token, idEvento);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, evento);
            }
            catch (EventoInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.EventoInvalidoCod, new Mensaje(MensajesParaFE.EventoInvalido));
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "getEvento", "Hubo un error al intentar obtener un evento, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorObtenerEventoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorObtenerEvento));
            }
        }


        /// <summary>
        /// Servicio para actualizar la descripcion de un recurso.
        /// </summary>
        /// <param name="descParam">Entension y descripcion a agregar.</param>
        /// <returns>Respuesta definida en el documento de interfaz.</returns>
        [HttpPost]
        [Route("eventos/actualizardescripcionrecurso")]
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
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "ActualizarDescripcionRecurso", "Hubo un error al intentar actualizar la descripcion, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorActualizarDescripcionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorActualizarDescripcion));
            }
        }


        [HttpPost]
        [Route("eventos/reportarhoraarribo")]
        [LogFilter]
        public DtoRespuesta ReportarHoraArribo(int idExtension)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }
                if (dbAL.reportarHoraArribo(token, idExtension))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (InvalidTokenException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "ReportarHoraArribo", "Hubo un error al intentar reportar la hora de arribo, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorReportarHoraArriboCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorReportarHoraArribo));
            }
        }

    }
}
