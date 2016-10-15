using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypeObject
{

    public class CodigosLog
    {
        //LOG  - Codigos de error.
        public const int ErrorIniciarSesionCod = 1001;
        public const int ErrorCerrarSesionCod = 2001;
        public const int ErrorAlGenerarToken = 4001;
        public const int ErrorConsumirServicioExternoCod = 3001;
        public const int LogAccionesCod = 9999;
        public const int LogLLamdosCod = 9998;
        public const int LogNotificacionesCod = 9997;
        public const int LogCapturarCambioEventoCod = 9997;


        /// <summary>
        /// Funcion interna para generar una descripcion amigable.
        /// </summary>
        /// <param name="codigo">Numero identificador del posible error capturado.</param>
        /// <returns>Descripcion amigable.</returns>
        private static string ObtenerDescripcionCodigo(int codigo)
        {
            switch (codigo)
            {
                case ErrorIniciarSesionCod:
                    return "Error al iniciar sesion.";
                default:
                    return "Error.";
            }
        }

        /// <summary>
        /// Metodo publico para obtener descripcion amigable de un error a partir de un codigo de error.
        /// </summary>
        /// <param name="cod">Codigo de error, utilizar las constantes de CodigosLogs<./param>
        /// <returns>Mensjae de error amigable.</returns>
        public static string GetDesciptionError(int cod) {
            return cod + ": " + ObtenerDescripcionCodigo(cod);
        }
        
    }
}
