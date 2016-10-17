﻿using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Emsys.LogicLayer
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Tiempo para el cual se desonectan usuarios (inactivos por mas de maxTime minutos). 
            const int maxTime = 12;
            // Tiempo cada el cual el servidor checkea por usuarios inactivos (cara refreshTime minutos).
            const int refreshTime = 6;
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
