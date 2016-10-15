using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{
    public class data
    {
        public data(string cod, string pk) {
            code = cod;
            primarykey = pk;    
        }

        public string code { get; set; }
        public string primarykey { get; set; }

    }
}
