using DataTypeObject;
using Emsys.LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Servicios
{
    public class ObtenerToken
    {
        /// <summary>
        /// Metodo para obtener el nombre del usuario desde un request.
        /// </summary>
        /// <param name="request">Request del llamado.</param>
        /// <returns>Nombre de usuario del llamado.</returns>
        public static string GetToken(HttpRequestMessage request)
        {
            try
            {
                IEnumerable<string> headerValues;
                var token = string.Empty;
                if (request.Headers.TryGetValues("auth", out headerValues))
                {
                    token = headerValues.FirstOrDefault();
                    return token;
                }
                return null;
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("", "", "Emsys.ServiceLayer", "LoginController", 0, "Login", "Hubo un error al intentar iniciar sesion, se adjunta excepcion: " + e.Message, Mensajes.ErrorAlGenerarToken);
                return null;
            }
        }
    }
}