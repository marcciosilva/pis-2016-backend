using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartup(typeof(Servicios.Startup))]

namespace Servicios
{
    /// <summary>
    /// Clase utilizada para configurar autenticacion OAuth.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Metodo de configuracion.
        /// </summary>
        /// <param name="app">Parametro.</param>
        public void Configuration(IAppBuilder app)
        {
            Console.WriteLine("prueba");
            // ConfigureOAuth(app);
        }
    }
}