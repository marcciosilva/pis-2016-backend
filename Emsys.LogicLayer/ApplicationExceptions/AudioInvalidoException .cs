using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.ApplicationExceptions
{
    public class AudioInvalidoException : Exception
    {
        public AudioInvalidoException()
        {
        }

        public AudioInvalidoException(string message)
        : base(message)
        {
        }

        public AudioInvalidoException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
