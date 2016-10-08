using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.ApplicationExceptions
{
    public class EventoInvalidoException : Exception
    {
        public EventoInvalidoException()
        {
        }

        public EventoInvalidoException(string message)
        : base(message)
        {
        }

        public EventoInvalidoException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
