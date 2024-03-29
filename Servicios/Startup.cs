﻿using System;
using System.Linq;
using System.Threading;
using Emsys.DataAccesLayer.Core;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Servicios.Startup))]

namespace Servicios
{
    /// <summary>
    /// Clase utilizada para configurar autenticacion OAuth.
    /// </summary>
    public partial class Startup
    {
        private static bool _iniciado = false;

        /// <summary>
        /// Metodo de configuracion.
        /// </summary>
        /// <param name="app">Parametro.</param>
        public void Configuration(IAppBuilder app)
        {
            try
            {
                if (!_iniciado)
                {
                    // Para iniciar la bd si no esta creada.
                    using (EmsysContext db = new EmsysContext())
                    {
                        db.Evento.ToList();
                    }
                    Thread UserAdminThread = new Thread(new ThreadStart(Emsys.LogicLayer.Program.Main));
                    UserAdminThread.IsBackground = true;
                    UserAdminThread.Start();

                    Thread DBObserverThread = new Thread(new ThreadStart(SqlDependecyProject.Program.Main));
                    DBObserverThread.IsBackground = true;
                    DBObserverThread.Start();

                    _iniciado = true;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}