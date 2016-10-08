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

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestBody = await request.Content.ReadAsStringAsync();
            string token = ObtenerToken.GetToken(request);
            IMetodos dbAL = new Metodos();
            dbAL.AgregarLog(token, GetClientIp(request), "", "", 0, "", "Request body: " + requestBody.ToString(), Mensajes.LogAccionesCod);
            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);
            if (result.Content != null)
            {
                var responseBody = await result.Content.ReadAsStringAsync();

                dbAL.AgregarLog(token, GetClientIp(request), "", "", 0, "", "Response body: " + responseBody.ToString(), Mensajes.LogAccionesCod);
            }

            return result;
        }


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