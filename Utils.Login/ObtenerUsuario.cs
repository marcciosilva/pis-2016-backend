using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emsys.DataAccesLayer.Model;
using System.Net.Http;

namespace Utils.Login
{
    public static class ObtenerUsuario
    {
        public static string ObtenerNombreUsuario(HttpRequestMessage request)
        {
            using (Emsys.DataAccesLayer.Core.EmsysContext db = new Emsys.DataAccesLayer.Core.EmsysContext())
            {
                IEnumerable<string> headerValues;
                var token = string.Empty;
                if (request.Headers.TryGetValues("auth", out headerValues))
                {
                    token = headerValues.FirstOrDefault();
                    token = token.Replace("Bearer ", "");
                    token = token.Replace("Bearer", "");
                    var user = db.Users.FirstOrDefault(u => u.Token == token);
                    if(user != null)
                        return user.NombreUsuario;
                }
                return null;
            }
        }
    }
}
