using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.ApplicationExceptions
{
    public class InvalidExtensionForUserException : Exception
    {
        public InvalidExtensionForUserException()
        {
        }
       

        public InvalidExtensionForUserException(string message)
        : base(message)
        {
        }

        public InvalidExtensionForUserException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
