using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Login
{
    public class TokenGenerator
    {
        private static int token = 0;

        public static string ObetenerToken() {
            token++;
            return token.ToString();
        }

    }
}
