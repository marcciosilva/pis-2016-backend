using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using Emsys.DataAccesLayer.Model;
using DataTypeObject;
using DataTypeObjetc;
using Newtonsoft.Json;

namespace Servicios.Filtros
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] PermisosEtiqueta;

        public CustomAuthorizeAttribute(params string[] permisos)
        {
            PermisosEtiqueta = permisos;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (IsAuthorized(actionContext))
                base.OnAuthorization(actionContext);
            else
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado))))
                };
                actionContext.Response = responseMessage;
            }
        }

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            // Voy a obtener el usuario del token.
            IEnumerable<string> salida;
            if (actionContext.Request.Headers.TryGetValues("auth", out salida))
            {
                var token = salida.FirstOrDefault();
                token = token.Replace("Bearer ", "");
                token = token.Replace("Bearer", "");

                using (Emsys.DataAccesLayer.Core.EmsysContext db = new Emsys.DataAccesLayer.Core.EmsysContext())
                {
                    var usuario = db.Users.Where(x => x.Token == token).FirstOrDefault();
                    if (usuario != null)
                    {
                        if (!PermisosEtiqueta.Any())
                        {
                            return true;
                        }
                        foreach (var item in PermisosEtiqueta)
                        {
                            foreach (ApplicationRole ar in usuario.ApplicationRoles)
                            {
                                foreach (Permiso p in ar.Permisos)
                                {
                                    if (item == p.Clave)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}