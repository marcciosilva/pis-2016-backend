using System.Web.Http;
using Emsys.ServiceLayer;
using Swashbuckle.Application;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Emsys.ServiceLayer
{
    public class SwaggerConfig
    {
        /// <summary>
        /// Metodo de configuracion para swaagger
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Servicios");
                    })
                .EnableSwaggerUi(c => { });
        }
    }
}
