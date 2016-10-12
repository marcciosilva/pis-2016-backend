using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.LogicLayer.ApplicationExceptions
{
    public class UsuarioNoAutorizadoException : Exception
    {
        public UsuarioNoAutorizadoException()
        {
        }

        public UsuarioNoAutorizadoException(string message)
        : base(message)
        {
        }

        public UsuarioNoAutorizadoException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
