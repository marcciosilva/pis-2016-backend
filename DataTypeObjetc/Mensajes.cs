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
