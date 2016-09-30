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
using Emsys.DataAccesLayer.Core;
using System.IdentityModel.Policy;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Web.Http.Dispatcher;
using Microsoft.AspNet.Identity;

namespace Emsys.ServiceLayer.Filtros
{

    public class LogFilter : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestBody = await request.Content.ReadAsStringAsync();
            Trace.WriteLine(requestBody);
            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);
            if (result.Content != null)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                var user = HttpContext.Current.User.Identity.GetUserId();
                Emsys.Logs.Log.AgregarLog(user, GetClientIp(request), "Modulo","Entidad",0,"accion", responseBody.ToString(), Emsys.Logs.Constantes.LogAcciones);                
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