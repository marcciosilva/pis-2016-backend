using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.ApplicationExceptions
{
    public class RecursoNoDisponibleException : Exception
    {
        public RecursoNoDisponibleException()
        {
        }

        public RecursoNoDisponibleException(string message)
        : base(message)
        {
        }

        public RecursoNoDisponibleException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
