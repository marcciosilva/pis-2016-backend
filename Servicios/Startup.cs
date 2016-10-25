﻿using Emsys.DataAccesLayer.Core;
using Hangfire;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using System.Threading;

[assembly: OwinStartup(typeof(Servicios.Startup))]

namespace Servicios
{
    /// <summary>
    /// Clase utilizada para configurar autenticacion OAuth.
    /// </summary>
    public partial class Startup
    {
        private static bool iniciado = false;
        /// <summary>
        /// Metodo de configuracion.
        /// </summary>
        /// <param name="app">Parametro.</param>
        public void Configuration(IAppBuilder app)
        {
            try
            {
                if (!iniciado)
                {
                    // Para iniciar la bd si no esta creada.
                    EmsysContext db = new EmsysContext();
                    db.Evento.ToList();

                    //GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
                    //app.UseHangfireDashboard();
                    //app.UseHangfireServer();
                    //RecurringJob.AddOrUpdate("ObserverDatabase",() => SqlDependecyProject.Program.Main(), Cron.Yearly);
                    //RecurringJob.Trigger("ObserverDatabase");
                    //RecurringJob.AddOrUpdate("UserManager",() => Emsys.LogicLayer.Program.Main(), Cron.Yearly);
                    //RecurringJob.Trigger("UserManager");

                    //Thread SQlMonitorThread = new Thread(new ThreadStart(SqlDependecyProject.Program.Main));
                    //SQlMonitorThread.IsBackground = true;
                    //SQlMonitorThread.Start();

                    //Thread UserAdminThread = new Thread(new ThreadStart(Emsys.LogicLayer.Program.Main));
                    //UserAdminThread.IsBackground = true;
                    //UserAdminThread.Start();

                    iniciado = true;
                }
            }
            catch (Exception)
            {

            }
        }
    }
}