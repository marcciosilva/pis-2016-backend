using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.ApplicationExceptions
{
    public class ImagenInvalidaException : Exception
    {
        public ImagenInvalidaException()
        {
        }

        public ImagenInvalidaException(string message)
        : base(message)
        {
        }

        public ImagenInvalidaException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
