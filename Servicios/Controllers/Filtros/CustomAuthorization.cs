using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using DataTypeObject;
using Newtonsoft.Json;
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

        /// <summary>
        /// Sobreescritura del metodo de autorizacion de AuthorizeAtribute
        /// </summary>
        /// <param name="actionContext">Contexto del resquest.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (IsAuthorized(actionContext))
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(2, new Mensaje(Mensajes.UsuarioNoAutenticado))))
                };
                actionContext.Response = responseMessage;
            }
        }

        /// <summary>
        /// Metodo que retorna si el usuario esta autorizdo a realizar la opreacion para la cual se utilizo la etiqueta.
        /// </summary>
        /// <param name="actionContext">Contexto del request.</param>
        /// <returns>Si esta autorizado el usuario o no.</returns>
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