using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObjetc
{

    public class Mensajes 
    {
        public const string Correcto = "Correcto";
        public const string UsuarioContraseñaInvalidos = "Usuario/Password inválido.";
        public const string UsuarioLogueado = "Usuario ya autenticado.";
        public const string UsuarioNoAutenticado = "Usuario no autenticado.";
        public const string UsuarioTieneOperacionesNoFinalizadas = "El usuario tiene una operación no finalizada.";
        public const string ErrorCerraSesion = "Ocurrio un error en el servidor al cerrar la sesion";
        public static int GetCodMenssage(string constante)
        {
            switch (constante)
            {
                case Correcto:
                    return 0;
                case UsuarioContraseñaInvalidos:
                    return 1;               
                case UsuarioLogueado:
                    return 6;
                case UsuarioNoAutenticado:
                    return 2;
                case UsuarioTieneOperacionesNoFinalizadas:
                    return 5;
                case ErrorCerraSesion:
                    return 500
                default: 
                    return 9999999;
            }
        }

        public string GetMenssage(Mensajes constante)
        {
            return constante.ToString();
        }
    }
}
