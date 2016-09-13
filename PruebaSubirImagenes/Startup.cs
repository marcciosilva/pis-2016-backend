using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PruebaSubirImagenes.Startup))]
namespace PruebaSubirImagenes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
