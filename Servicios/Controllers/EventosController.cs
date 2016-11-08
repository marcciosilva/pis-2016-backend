using System;
using System.Collections.Generic;
using System.Web.Http;
using DataTypeObject;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Servicios.Filtros;

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
            catch (TokenInvalidoException)
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
        [CustomAuthorizeAttribute("obtenerEvento")]
        [HttpGet]
        [LogFilter]
        [Route("eventos/obtener")]
        public DtoRespuesta GetEvento(int idEvento)
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
            catch (TokenInvalidoException)
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
        [CustomAuthorizeAttribute("actualizarDescripcionRecurso")]
        [HttpPost]
        [Route("eventos/actualizardescripcionrecurso")]
        [LogFilter]
        public DtoRespuesta ActualizarDescripcionRecurso([FromBody] DtoActualizarDescripcion descParam)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                if (dbAL.ActualizarDescripcionRecurso(descParam, token))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }

                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (UsuarioNoAutorizadoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "ActualizarDescripcionRecurso", "Hubo un error al intentar actualizar la descripcion, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorActualizarDescripcionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorActualizarDescripcion));
            }
        }


        /// <summary>
        /// Servicio para actualizar la descripcion de un recurso.
        /// </summary>
        /// <param name="descParam">Entension y descripcion a agregar.</param>
        /// <returns>Respuesta definida en el documento de interfaz.</returns>
        [HttpPost]
        [Route("eventos/actualizardescripcionrecursooffline")]
        [LogFilter]
        public DtoRespuesta ActualizarDescripcionRecursoOffline([FromBody] DtoActualizarDescripcionOffline descParam)
        {
            IMetodos dbAL = new Metodos();
            try
            {
                if (dbAL.ActualizarDescripcionRecursoOffline(descParam))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }

                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (CredencialesInvalidasException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioContraseñaInvalidosCod, new Mensaje(MensajesParaFE.UsuarioContraseñaInvalidos));
            }
            catch (ExtensionInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (RecursoInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.RecursoInvalidoCod, new Mensaje(MensajesParaFE.RecursoInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(descParam.userData.username, "", "Emsys.ServiceLayer", "EventosController", 0, "ActualizarDescripcionRecursoOffline", "Hubo un error al intentar actualizar la descripcion, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorActualizarDescripcionOfflineCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorActualizarDescripcion));
            }
        }

        /// <summary>
        /// Servicio para reportar la hora que un recurso arriba a un evento.
        /// </summary>
        /// <param name="idExtension">Extension que atiende el recurso</param>
        /// <returns>DtoRespuesta indicando el resultado</returns>
        [CustomAuthorizeAttribute("reportarHoraArribo")]
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
            catch (ArriboPrevioException)
            {
                return new DtoRespuesta(MensajesParaFE.ArriboPrevioCod, new Mensaje(MensajesParaFE.ArriboPrevio));
            }
            catch (ExtensionInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "ReportarHoraArribo", "Hubo un error al intentar reportar la hora de arribo, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorReportarHoraArriboCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorReportarHoraArribo));
            }
        }

        /// <summary>
        /// Servicio para obtener informacion necesaria para crear un evento.
        /// </summary>
        /// <returns>Lista de zonas y sectores, categorias y departamentos</returns>
        [CustomAuthorizeAttribute("infoCreacionEvento")]
        [HttpGet]
        [Route("eventos/infocreacionevento")]
        [LogFilter]
        public DtoRespuesta GetInfoCreacionEvento()
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                var info = dbAL.getInfoCreacionEvento(token);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, info);
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "GetInfoCreacionEvento", "Hubo un error al intentar obtener la informacion para crear un evento, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorInfoCreacionEventoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorInfoCreacionEvento));
            }
        }

        /// <summary>
        /// Servicio para crear un evento nuevo.
        /// </summary>
        /// <param name="evento">Dto con los datos del evento a crear</param>
        /// <returns>DtoRespuesta indicando el resultado</returns>
        [CustomAuthorizeAttribute("crearEvento")]
        [HttpPost]
        [Route("eventos/crearevento")]
        [LogFilter]
        public DtoRespuesta CrearEvento([FromBody] DtoEvento evento)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                if (dbAL.crearEvento(token, evento))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }

                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorCrearEvento));
            }
            catch (SeleccionZonasInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.EventoSinZonasCod, new Mensaje(MensajesParaFE.EventoSinZonas));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ArgumentoInvalidoException e)
            {
                return new DtoRespuesta(MensajesParaFE.ArgumentoInvalidoCod, new Mensaje(MensajesParaFE.ArgumentoInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "CrearEvento", "Hubo un error al intentar crear un evento, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorCrearEventoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorCrearEvento));
            }
        }

        /// <summary>
        /// Servicio para despachar una extension.
        /// </summary>
        /// <param name="idExtension">Extension a despachar</param>
        /// <returns>Dto indicando si se realizo con exito</returns>
        [CustomAuthorizeAttribute("despacharExtension")]        
        [HttpPost]
        [Route("eventos/tomarextension")]
        [LogFilter]                
        public DtoRespuesta TomarExtension(int idExtension)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                dbAL.tomarExtension(token, idExtension);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (UsuarioNoAutorizadoException e)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "TomarExtension", "Hubo un error al intentar tomar la extension, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorTomarExtensionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorTomarExtension));
            }
        }

        /// <summary>
        /// Servicio para liberar una extension actualmente despachada.
        /// </summary>
        /// <param name="idExtension">Id de la extension a liberar</param>
        /// <returns>Si se libero con exito</returns>
        [CustomAuthorizeAttribute("despacharExtension")]
        [HttpPost]
        [Route("eventos/liberarextension")]
        [LogFilter]
        public DtoRespuesta LiberarExtension(int idExtension)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                if (dbAL.liberarExtension(token, idExtension))
                {
                    return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
                }

                return new DtoRespuesta(MensajesParaFE.ErrorLiberarExtensionCod, new Mensaje(MensajesParaFE.ErrorLiberarExtension));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "LiberarExtension", "Hubo un error al intentar liberar la extension, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorLiberarExtensionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorLiberarExtension));
            }
        }

        /// <summary>
        /// Servicio para obtener los recursos usados y disponibles para una extension.
        /// </summary>
        /// <param name="idExtension">Id de la extension a consultar</param>
        /// <returns>DtoRespuesta con una lista de los recursos asignados a la extension y otra con los recursos disponibles</returns>
        [CustomAuthorizeAttribute("gestionarRecursosExtension")]
        [HttpGet]
        [Route("eventos/getrecursosextension")]
        [LogFilter]
        public DtoRespuesta GetRecursosExtension(int idExtension)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                var resp = dbAL.getRecursosExtension(token, idExtension);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, resp);
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "GetRecursosExtension", "Hubo un error al intentar obtener los recursos de la extension, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorGetRecursosExtensionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorGetRecursosExtension));
            }
        }

        /// <summary>
        /// Servicio para asignar o retirar recursos de una extension.
        /// </summary>
        /// <param name="recursos">Dto con una lista de recursos a agregar ("recursosAsignados") y una lista de recursos a retirar ("recursosNoAsignados") y el id de la extension</param>
        /// <returns>Dto indicando el exito o no de la operacin</returns>
        [CustomAuthorizeAttribute("gestionarRecursosExtension")]
        [HttpPost]
        [Route("eventos/gestionarrecursos")]
        [LogFilter]
        public DtoRespuesta GestionarRecursos([FromBody] DtoRecursosExtension recursos)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                dbAL.gestionarRecursos(token, recursos);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (ArgumentoInvalidoException e)
            {
                return new DtoRespuesta(MensajesParaFE.ArgumentoInvalidoCod, new Mensaje(MensajesParaFE.ArgumentoInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "GestionarRecursos", "Hubo un error al intentar gestionar recursos de una extension, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorGestionarRecursosCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorGestionarRecursos));
            }
        }

        /// <summary>
        /// Servicio para actualizar la segunda categoria de una extension.
        /// </summary>
        /// <param name="idExtension">El id de la extension a actualizar</param>
        /// <param name="idCategoria">El id de la nueva categoria (-1 indica eliminar categoria actual unicamente)</param>
        /// <returns>Exito o no</returns>
        [CustomAuthorizeAttribute("actualizarSegundaCategoria")]
        [HttpPost]
        [Route("eventos/actualizarsegundacategoria")]
        [LogFilter]
        public DtoRespuesta ActualizarSegundaCategoria(int idExtension, int idCategoria)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                dbAL.actualizarSegundaCategoria(token, idExtension, idCategoria);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }            
            catch (CategoriaInvalidaException)
            {
                return new DtoRespuesta(MensajesParaFE.CategoriaInvalidaCod, new Mensaje(MensajesParaFE.CategoriaInvalida));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "ActualizarSegundaCategoria", "Hubo un error al intentar actualizar la segunda categoria de una extension, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorActualizarSegundaCategoriaCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorActualizarSegundaCategoria));
            }
        }

        /// <summary>
        /// Servicio para obtener las zonas libres (disponibles para avrir extensiones nuevas) de un evento.
        /// </summary>
        /// <param name="idExtension">Id de la extension a consultar</param>
        /// <returns>Lista de dtos de las zonas libres</returns>
        [CustomAuthorizeAttribute("abrirExtension")]
        [HttpGet]
        [Route("eventos/getzonaslibresevento")]
        [LogFilter]
        public DtoRespuesta GetZonasLibresEvento(int idExtension)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                var zonas = dbAL.getZonasLibresEvento(token, idExtension);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, zonas);
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "GetZonasLibresEvento", "Hubo un error al intentar actualizar obtener las zonas libres de un evento, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorGetZonasLibresEventoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorGetZonasLibresEvento));
            }
        }

        /// <summary>
        /// Servicio para abrir una extension nueva en un evento.
        /// </summary>
        /// <param name="idExtension">Id de una extension despahcada por el usuario en ese evento</param>
        /// <param name="idZona">Id de la zona para la cual se abre la extension</param>
        /// <returns>Exito o fracaso</returns>
        [CustomAuthorizeAttribute("abrirExtension")]
        [HttpPost]
        [Route("eventos/abrirextension")]
        [LogFilter]
        public DtoRespuesta AbrirExtension(int idExtension, int idZona)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                dbAL.abrirExtension(token, idExtension, idZona);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (ZonaInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ZonaInvalidaCod, new Mensaje(MensajesParaFE.ZonaInvalida));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "AbrirExtension", "Hubo un error al intentar abrir una extension para un evento, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorAbrirExtensionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorAbrirExtension));
            }
        }

        /// <summary>
        /// Servicio para cerrar una extension de un evento.
        /// </summary>
        /// <param name="idExtension">Id de la extension a cerrar</param>
        /// <returns>Resultado de la operacion</returns>
        [CustomAuthorizeAttribute("cerrarExtension")]
        [HttpPost]
        [Route("eventos/cerrarextension")]
        [LogFilter]
        public DtoRespuesta CerrarExtension(int idExtension)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                dbAL.cerrarExtension(token, idExtension);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (EventoNoEnviadoException e)
            {
                return new DtoRespuesta(MensajesParaFE.EventoNoEnviadoCod, new Mensaje(MensajesParaFE.EventoNoEnviado));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "CerrarExtension", "Hubo un error al intentar cerrar una extension de un evento, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorCerrarExtensionCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorCerrarExtension));
            }
        }

        /// <summary>
        /// Servicio para actualziar la descripcion de despachador.
        /// </summary>
        /// <param name="descParam">Dto con el id de la extension y la descripcion a actualizar</param>
        /// <returns>Resultado de la accion</returns>
        [CustomAuthorizeAttribute("actualizarDescripcionDespachador")]
        [HttpPost]
        [Route("eventos/actualizardescripciondespachador")]
        [LogFilter]
        public DtoRespuesta ActualizarDescripcionDespachador([FromBody] DtoActualizarDescripcion descParam)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
                }

                dbAL.actualizarDescripcionDespachador(token, descParam);
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, new Mensaje(MensajesParaFE.Correcto));
            }
            catch (TokenInvalidoException)
            {
                return new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado));
            }
            catch (ExtensionInvalidaException e)
            {
                return new DtoRespuesta(MensajesParaFE.ExtensionInvalidaCod, new Mensaje(MensajesParaFE.ExtensionInvalida));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "EventosController", 0, "ActualizarDescripcionDespachador", "Hubo un error al intentar actualizar la descripcion, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorActualizarDescripcionDespachadorCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorActualizarDescripcionDespachador));
            }
        }
    }
}
