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
using Utils.Login;

namespace Servicios.Controllers
{
    public class ServicioExternoController : ApiController
    {
        //[CustomAuthorizeAttribute()]
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

                return new DtoRespuesta(0, result);
            }
            catch (SoapException e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError(ObtenerToken.GetToken(Request), "", "Emsys.ServiceLayer", "CerrarSesionController", 0, "ConsumirServicioExterno", "Hubo un error al intentar consumir el servicio externo, se adjunta excepcion: " + e.Message, Mensajes.ErrorConsumirServicioExternoCod);
                return new DtoRespuesta(8, new Mensaje(Mensajes.ServicioExternoNoDisponible));
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError(ObtenerToken.GetToken(Request), "", "Emsys.ServiceLayer", "ServicioExternoController", 0, "ConsumirServicioExterno", "Hubo un error al intentar consumir el servicio externo, se adjunta excepcion: " + e.Message, Mensajes.ErrorConsumirServicioExternoCod);
                return new DtoRespuesta(500, new Mensaje(Mensajes.ErrorCerraSesion));
            }
        }
    }
}
