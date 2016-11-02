using System;
using System.Threading;
using System.Web.Configuration;

namespace Emsys.LogicLayer
{
    public class Program
    {
        public static void Main()
        {
            // Tiempo para el cual se desonectan usuarios (inactivos por mas de "maxTime" minutos). 
            var maxTime = Convert.ToInt32(WebConfigurationManager.AppSettings["maxTime"]);

            // Tiempo cada el cual el servidor checkea por usuarios inactivos (cada "refreshTime" minutos).
            var refreshTime = Convert.ToInt32(WebConfigurationManager.AppSettings["refreshTime"]);

            Console.WriteLine("Started...");
            IMetodos logica = new Metodos();
            while (true)
            {
                logica.desconectarAusentes(maxTime);
                Console.WriteLine("Sleeping...");
                Thread.Sleep(refreshTime * 60 * 1000);
            }
        }
    }
}
