using System;

namespace Emsys.LogicLayer.Utils
{
    class TokenGenerator
    {
        /// <summary>
        /// Genera un token de autenticacion nuevo.
        /// </summary>
        /// <returns>Nuevo token</returns>
        public static string ObtenerToken()
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return token.ToString();
        }
    }
}
