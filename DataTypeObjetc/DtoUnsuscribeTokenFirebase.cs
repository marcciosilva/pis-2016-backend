using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{
    public class DtoUnsuscribeTokenFirebase
    {
        public DtoUnsuscribeTokenFirebase(string topic, string tokenFirebase)
        {
            to = topic;
            registration_tokens = new string[1];
            registration_tokens[0] = tokenFirebase;
        }
        public string[] registration_tokens { get; set; }

        public string to { get; set; }
    }
}
