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
using Utils.Login;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;

namespace Emsys.ServiceLayer.Filtros
{

    public class LogFilter : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestBody = await request.Content.ReadAsStringAsync();
            var user = ObtenerUsuario.ObtenerNombreUsuario(request);// HttpContext.Current.User.Identity.GetUserId();

            //var nombreControlador =ObtenerNombreControlador(request);

            Emsys.Logs.Log.AgregarLog(user, GetClientIp(request), "Modulo", "Entidad", 0, "accion", "Request body: " + requestBody.ToString(), Emsys.Logs.Constantes.LogAcciones);
            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);
            if (result.Content != null)
            {
                var responseBody = await result.Content.ReadAsStringAsync();

                Emsys.Logs.Log.AgregarLog(user, GetClientIp(request), "Modulo", "Entidad", 0, "accion", "Response body: " + responseBody.ToString(), Emsys.Logs.Constantes.LogAcciones);
            }

            return result;
        }

        private string ObtenerNombreControlador(HttpRequestMessage request)
        {
            try
            {
                var attributedRoutesData = request.GetRouteData().GetSubRoutes();
                var subRouteData = attributedRoutesData.FirstOrDefault();
                var actions = (ReflectedHttpActionDescriptor[])subRouteData.Route.DataTokens["actions"];
                var controllerName = actions[0].ControllerDescriptor.ControllerName;
                return controllerName;
            }
            catch (Exception e)
            {
                return "No tengo nombre controlador";
            }
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