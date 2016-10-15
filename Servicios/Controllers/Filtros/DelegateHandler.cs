using System;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Emsys.LogicLayer;
using DataTypeObject;
using Servicios;

namespace Emsys.ServiceLayer.Filtros
{

    public class DelegateHandler : DelegatingHandler
    {
        /// <summary>
        /// Metodo que se dispara automaticamente al realizarse un request. Util para loguear el request y el response.
        /// </summary>
        /// <param name="request">Request de la llamada.</param>
        /// <param name="cancellationToken">No se usa.</param>
        /// <returns>Una tarea para que se ejecute esta logica de forma asincronica.</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestBody = await request.Content.ReadAsStringAsync();
            string token = ObtenerToken.GetToken(request);
            IMetodos dbAL = new Metodos();
            dbAL.AgregarLog(token, GetClientIp(request), "", "", 0, "", "Request body: " + requestBody.ToString(), CodigosLog.LogAccionesCod);
            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);
            if (result.Content != null)
            {
                var responseBody = await result.Content.ReadAsStringAsync();

                dbAL.AgregarLog(token, GetClientIp(request), "", "", 0, "", "Response body: " + responseBody.ToString(), CodigosLog.LogAccionesCod);
            }
            return result;
        }

        /// <summary>
        /// Funcion para obtener la identificacion del dispositivo.
        /// </summary>
        /// <param name="request">Request del llamado al servicio.</param>
        /// <returns>Identificacion del dispositivo que realizo el request.</returns>
        private string GetClientIp(HttpRequestMessage request)
        {
            String ip;
            if (!String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]))
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
    }
}