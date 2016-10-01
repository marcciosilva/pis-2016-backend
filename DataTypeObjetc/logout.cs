using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObjetc
{
    public class Logout: Response
    {
        public string msg { get; set; }

        public Logout(string message) {
            msg = message;
        }
    }
}
