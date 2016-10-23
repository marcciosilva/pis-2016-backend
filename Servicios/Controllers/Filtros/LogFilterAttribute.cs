using DataTypeObject;
using Emsys.LogicLayer;
using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;

namespace Servicios.Filtros
{
    public class LogFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        /// Sobre escritura del metodo para que se llama cuando se ejecuta un metodo.
        /// </summary>
        /// <param name="actionContext">Contexto.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Method == HttpMethod.Post)
            {
                var postData = actionContext.ActionArguments;
                //// Do logging here.
            }

            var actionName = actionContext.ActionDescriptor.ActionName;
            var controller = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string token = ObtenerToken.GetToken(actionContext.Request);
            IMetodos dbAL = new Metodos();
            dbAL.AgregarLog(token, GetIp(actionContext.Request), "ServiceLayer", "", 0, controller + "Controller" + "/" + actionName, "Se llamo al metodo", CodigosLog.LogAccionesCod);
        }

        /// <summary>
        /// Metodo que devulve el identificador del dispositivo.
        /// </summary>
        /// <param name="request">Resuqest del llamado del servicio.</param>
        /// <returns>Retorna el identificador del dispositivo.</returns>
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