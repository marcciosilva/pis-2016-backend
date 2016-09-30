using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emsys.Logs
{
    public class Constantes
    {
        #region Multimedia-Controller
        public const int ErrorObtenerArchivoMultimedia = 1000;
        #endregion

        public const int LogAcciones = 9999;

        public static string getMessageError(int idError)//Dejar un punto y espacio al final de los mensajes para que quede mas lindo en caso de anidar luego otro mensaje.
        {
            switch (idError)
            {
                case ErrorObtenerArchivoMultimedia:
                    return "Error al obtener el archivo multimedia.";
                case LogAcciones:
                    return "Se logueo una accion.";
                default:
                    return "Error interno, cancele la operacion y vuelva a intentarlo. ";

            }
        }

        public static string getCodeAndMessageError(int idError)
        {
            return idError + " - " + getMessageError(idError);
        }
    }
}
