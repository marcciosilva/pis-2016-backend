using DataTypeObject;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;
using Servicios.Filtros;
using Servicios.ServicioExterno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services.Protocols;

namespace Servicios.Controllers
{
    public class ServicioExternoController : ApiController
    {
        /// <summary>
        /// Servicio que solicita la llamda al servicio externo.
        /// </summary>
        /// <param name="consulta">Datos para realizar la consulta al servicio externo.</param>
        /// <returns>Retorna la respuesta definida en el documento de interfaz.</returns>
        [CustomAuthorizeAttribute("consumirServicioExterno")]
        [HttpPost]
        [LogFilter]
        [Route("servicioexterno")]
        public DtoRespuesta ConsumirServicioExterno([FromBody] DtoConsultaExterna consulta)
        {
            try
            {
                ServiciosSoapClient serv = new ServiciosSoapClient();
                ArrayOfString[] respuesta = serv.Servicio1(consulta.param1, consulta.param2, consulta.param3);
                ICollection<DtoRespuestaExterna> result = new List<DtoRespuestaExterna>();
                foreach (ArrayOfString item in respuesta)
                {
                    if (item.Count() == 10)
                    {
                        DtoRespuestaExterna itemResp = new DtoRespuestaExterna(item[0], item[1], item[2], item[3], item[4], item[5], item[6], item[7], item[8], item[9]);
                        result.Add(itemResp);
                    }
                }
                return new DtoRespuesta(MensajesParaFE.CorrectoCod, result);
            }
            catch (SoapException e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError(ObtenerToken.GetToken(Request), "", "Emsys.ServiceLayer", "ServicioExternoController", 0, "ConsumirServicioExterno", "Hubo un error al intentar consumir comunicarse con el servidor externo, se adjunta excepcion: " + e.Message, MensajesParaFE.ServicioExternoNoDisponibleCod);
                return new DtoRespuesta(MensajesParaFE.ServicioExternoNoDisponibleCod, new Mensaje(MensajesParaFE.ServicioExternoNoDisponible));
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError(ObtenerToken.GetToken(Request), "", "Emsys.ServiceLayer", "ServicioExternoController", 0, "ConsumirServicioExterno", "Hubo un error al intentar consumir el servicio externo, se adjunta excepcion: " + e.Message, MensajesParaFE.ErrorConsumirServicioExternoCod);
                return new DtoRespuesta(MensajesParaFE.ErrorCod, new Mensaje(MensajesParaFE.ErrorConsumirServicioExterno));
            }
        }
    }
}
