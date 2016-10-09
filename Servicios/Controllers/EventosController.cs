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
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }                
                ICollection<DtoEvento> eventos = dbAL.listarEventos(token);
                return  new DtoRespuesta(0, eventos);
            }
            catch (InvalidTokenException e)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "ListarEventosController", 0, "ListarEventos", "Hubo un error al intentar listar eventos de un usuario, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }       
        }


        //[CustomAuthorizeAttribute("verInfoEvento")]
        [CustomAuthorizeAttribute()]
        [HttpGet]
        [LogFilter]
        [Route("eventos/obtener")]
        public DtoRespuesta VerInfoEvento(int idEvento)
        {
            IMetodos dbAL = new Metodos();
            string token = ObtenerToken.GetToken(Request);
            try
            {
                if (token == null)
                {
                    return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
                }
                DtoEvento evento = dbAL.verInfoEvento(token, idEvento);
                return new DtoRespuesta(0, evento);
            }
            catch(EventoInvalidoException e)
            {
                return new DtoRespuesta(9, new Mensaje(Mensajes.EventoInvalido));
            }
            catch (InvalidTokenException e)
            {
                return new DtoRespuesta(2, new Mensaje(Mensajes.TokenInvalido));
            }
            catch (Exception e)
            {
                dbAL.AgregarLogError(token, "", "Emsys.ServiceLayer", "ListarEventosController", 0, "ListarEventos", "Hubo un error al intentar listar eventos de un usuario, se adjunta excepcion: " + e.Message, Mensajes.ErrorIniciarSesionCod);
                return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }
        }
    }
}
