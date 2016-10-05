using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.IdentityModel.Policy;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Web.Http.Dispatcher;
using Microsoft.AspNet.Identity;
using Utils.Login;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using Emsys.LogicLayer;

namespace Emsys.ServiceLayer.Filtros
{

    public class DelegateHandler : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestBody = await request.Content.ReadAsStringAsync();
            string token = ObtenerToken.GetToken(request);
            string user = null;
            if (token != null)
            {
                IMetodos dbAL = new Metodos();
                user = dbAL.getNombreUsuario(token);
            }    

            Emsys.Logs.Log.AgregarLog(user, GetClientIp(request), "", "", 0, "", "Request body: " + requestBody.ToString(), Emsys.Logs.Constantes.LogAcciones);
            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);
            if (result.Content != null)
            {
                var responseBody = await result.Content.ReadAsStringAsync();

                Emsys.Logs.Log.AgregarLog(user, GetClientIp(request), "", "", 0, "", "Response body: " + responseBody.ToString(), Emsys.Logs.Constantes.LogAcciones);
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