using System.Web.Http;

namespace Servicios.App_Start
{
    /// <summary>
    /// Utilizada para restringir acceso a todo endpoint menos el de OAuth.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Filtro para menejar los permisos por roles.
        /// </summary>
        /// <param name="config">Atributo del resquest.</param>
        public static void Configure(HttpConfiguration config)
        {
            config.Filters.Add(new AuthorizeAttribute());
        }
    }
}