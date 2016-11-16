using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using DataTypeObject;
using Emsys.LogicLayer;
using Newtonsoft.Json;
using Emsys.LogicLayer.ApplicationExceptions;

namespace Servicios.Filtros
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _permisosEtiqueta;

        /// <summary>
        /// Metodo encargado de guardar los datos de la equita como permisos para comparar en OnAuthorization.
        /// </summary>
        /// <param name="permisos"></param>
        public CustomAuthorizeAttribute(params string[] permisos)
        {
            _permisosEtiqueta = permisos;
        }

        /// <summary>
        /// Sobreescritura del metodo de autorizacion de AuthorizeAtribute
        /// </summary>
        /// <param name="actionContext">Contexto del resquest.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (IsAuthorized(actionContext))
                {
                    base.OnAuthorization(actionContext);
                }
                else
                {
                    HttpResponseMessage responseMessage = new HttpResponseMessage()
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutorizadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutorizado))))
                    };
                    actionContext.Response = responseMessage;
                }
            }
            catch (TokenInvalidoException)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new DtoRespuesta(MensajesParaFE.UsuarioNoAutenticadoCod, new Mensaje(MensajesParaFE.UsuarioNoAutenticado))))
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
                throw new TokenInvalidoException();
            }

            IMetodos dbAL = new Metodos();
            return dbAL.autorizarUsuario(token, _permisosEtiqueta);            
        }
    }
}