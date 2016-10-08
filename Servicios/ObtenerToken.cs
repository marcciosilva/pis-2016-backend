using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Servicios
{
    public class ObtenerToken
    {
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
                return null;
            }
        }
    }
}