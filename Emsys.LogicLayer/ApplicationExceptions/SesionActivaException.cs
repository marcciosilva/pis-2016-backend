using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.ApplicationExceptions
{
    public class SesionActivaException : Exception
    {
        public SesionActivaException()
        {
        }

        public SesionActivaException(string message)
        : base(message)
        {
        }

        public SesionActivaException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
