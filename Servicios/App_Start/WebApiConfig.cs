using Newtonsoft.Json.Converters;
using System.Web.Http;

namespace Emsys.ServiceLayer
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Metodo que registra la configuracion de Ruteo y filtros para los servicios.
        /// </summary>
        /// <param name="config">Parametro de configuracion.</param>
        public static void Register(HttpConfiguration config)
        {
            //config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
            //new IsoDateTimeConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
             new IsoDateTimeConverter());
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new Emsys.ServiceLayer.Filtros.DelegateHandler());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
