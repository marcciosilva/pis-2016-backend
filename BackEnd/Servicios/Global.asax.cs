//#define CLIENT
using Emsys.DataAccesLayer.Core;
using Servicios.App_Start;
using System.Data.Entity;
using System.Diagnostics;
using System.Web.Http;

namespace Emsys.ServiceLayer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Database.SetInitializer(new Initializer());
            setUpFilters();
        }

        /// <summary>
        /// Si se usa el cliente en Android, se activan los filtros que bloquean
        /// el acceso a cualquier endpoint menos el de OAuth. Si la directiva
        /// no esta definida, se asume que se testea desde Swagger y no se
        /// bloquea ningun endpoint.
        /// </summary>
        [Conditional("CLIENT")]
        private void setUpFilters()
        {
            GlobalConfiguration.Configure(FilterConfig.Configure);
        }


    }
}
