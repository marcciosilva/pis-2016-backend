using System;
using System.Threading;
using System.Web.Configuration;

namespace Emsys.LogicLayer
{
    public class Program
    {
        /// <summary>
        /// Metodo de la aplicacion de consola encargada de desloguear a los usuarios.
        /// </summary>
        public static void Main()
        {
            // Tiempo para el cual se desonectan usuarios (inactivos por mas de "maxTime" minutos). 
            var maxTime = Convert.ToInt32(WebConfigurationManager.AppSettings["maxTime"]);
            // Tiempo cada el cual el servidor checkea por usuarios inactivos (cada "refreshTime" minutos).
            var refreshTime = Convert.ToInt32(WebConfigurationManager.AppSettings["refreshTime"]);
            // Tiempo tras el cual se desconectan a usuarios del sistema (luego de "duracionTurno" horas).
            var duracionTurno = Convert.ToInt32(WebConfigurationManager.AppSettings["duracionTurno"]);
            Console.WriteLine("Started...");
            IMetodos logica = new Metodos();
            while (true)
            {
                try
                {
                    logica.desconectarAusentes(maxTime, duracionTurno);
                    Console.WriteLine("Sleeping...");
                    Thread.Sleep(refreshTime * 60 * 1000);
                }
                catch (Exception e)
                {
                    logica.AgregarLogError("", "", "Emsys.LogicLayer", "Main", 0, "desconectarAusentes", "Hubo un error al intentar desconectar usuarios ausentes: " + e.Message, 9991);
                }
            }
        }
    }
}
