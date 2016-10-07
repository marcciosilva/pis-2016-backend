using DataTypeObject;
using Emsys.LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using Utils.Login;

namespace Servicios.Filtros
{
    public class LogFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Method == HttpMethod.Post)
            {
                var postData = actionContext.ActionArguments;
                //do logging here
            }
            var actionName = actionContext.ActionDescriptor.ActionName;
            var controller = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string token = ObtenerToken.GetToken(actionContext.Request);
            IMetodos dbAL = new Metodos();
            dbAL.AgregarLog(token, GetIp(actionContext.Request), "ServiceLayer", "", 0, controller+"Controller"+"/"+actionName, "Se llamo al metodo", Mensajes.LogAccionesCod);
        }
        private string GetIp(HttpRequestMessage request)
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