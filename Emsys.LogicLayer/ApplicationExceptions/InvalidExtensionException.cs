using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.ApplicationExceptions
{
    public class InvalidExtensionException : Exception
    {
        public InvalidExtensionException()
        {
        }
       
        public InvalidExtensionException(string message)
        : base(message)
        {
        }

        public InvalidExtensionException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
