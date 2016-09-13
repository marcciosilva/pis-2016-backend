using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Servicios.App_Start
{
    /// <summary>
    /// Utilizada para restringir acceso a todo endpoint menos el de OAuth.
    /// </summary>
    public class FilterConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            config.Filters.Add(new AuthorizeAttribute());
        }
    }
}