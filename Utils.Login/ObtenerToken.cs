using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emsys.DataAccesLayer.Model;
using System.Net.Http;

namespace Utils.Login
{
    public static class ObtenerToken
    {
        public static string GetToken(HttpRequestMessage request)
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
    }
}
