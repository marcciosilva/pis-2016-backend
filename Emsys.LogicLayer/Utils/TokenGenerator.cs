using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
