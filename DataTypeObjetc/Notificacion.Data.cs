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
            codigo = cod;
            primarykey = pk;    
        }

        public string codigo { get; set; }
        public string primarykey { get; set; }

    }
}
