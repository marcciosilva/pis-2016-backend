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
    public class ListarEventosController : ApiController
    {
        [CustomAuthorizeAttribute("listarEventos")]
        [HttpGet]
        [Route("events")]
        public DtoRespuesta ListarEventos()
        {
            try
            {
                IMetodos dbAL = new Metodos();
                ICollection<DtoEvento> eventos = dbAL.listarEventos(ObtenerUsuario.ObtenerNombreUsuario(Request));
                return  new DtoRespuesta(0, eventos);
            }
            catch (Exception e)
            {
                Emsys.Logs.Log.AgregarLogError(ObtenerUsuario.ObtenerNombreUsuario(Request), "", "Emsys.ServiceLayer", "ListarEventosController", 0, "ListarEventos", "Hubo un error al intentar listar eventos de un usuario, se adjunta excepcion: " + e.Message, Emsys.Logs.Constantes.ErrorIniciarSesion);
                return new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado));
            }       
        }
    }
}
