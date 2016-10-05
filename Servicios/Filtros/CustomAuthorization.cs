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
using DataTypeObject;
using DataTypeObjetc;
using Newtonsoft.Json;
using Utils.Login;
using Emsys.LogicLayer;

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
            string token = ObtenerToken.GetToken(actionContext.Request);
            if (token == null)
            {
                return false;
            }
            IMetodos dbAL = new Metodos();
            return dbAL.autorizarUsuario(token, PermisosEtiqueta);            
        }
    }
}