using System;

namespace Emsys.LogicLayer.Utils
{
    class TokenGenerator
    {
        public static string ObtenerToken()
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return token.ToString();
        }
    }
}
