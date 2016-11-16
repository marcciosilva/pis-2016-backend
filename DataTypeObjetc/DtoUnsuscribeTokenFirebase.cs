using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{
    public class DtoUnsuscribeTokenFirebase
    {
        /// <summary>
        /// Constructor para envio de informacion de dessuscripcion de topics.
        /// </summary>
        /// <param name="topic">Nombre del canal.</param>
        /// <param name="tokenFirebase">Identificador de dispostivo de firebase.</param>
        public DtoUnsuscribeTokenFirebase(string topic, string tokenFirebase)
        {
            this.to = topic;
            this.registration_tokens = new string[1];
            this.registration_tokens[0] = tokenFirebase;
        }
        public string[] registration_tokens { get; set; }

        public string to { get; set; }
    }
}
