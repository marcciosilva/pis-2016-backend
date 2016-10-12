using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.ApplicationExceptions
{
    public class VideoInvalidoException : Exception
    {
        public VideoInvalidoException()
        {
        }

        public VideoInvalidoException(string message)
        : base(message)
        {
        }

        public VideoInvalidoException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
