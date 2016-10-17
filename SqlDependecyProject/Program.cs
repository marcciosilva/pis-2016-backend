namespace SqlDependecyProject
{
    using System;
    using TableDependency.Mappers;
    using TableDependency.SqlClient;
    using TableDependency.Enums;
    using Emsys.DataAccesLayer.Model;
    using Emsys.DataAccesLayer.Core;
    using System.Linq;
    using DataTypeObject;
    using Emsys.LogicLayer;
    using System.Threading;

    public class Program
    {
        private enum TablaMonitorar { Eventos, Extensiones }
        
        /// <summary>
        /// Metodo principal para el proyecto ObserverDatabase encargado de manejar las notificaciones de los cambios de la bd.
        /// </summary>
        /// <param name="args">No utilizado.</param>
        public static void Main(string[] args)
        {
            // Para cada tabla a ser monitoreada hay que iniciar un thread.
            Thread workerThread = new Thread(new ThreadStart(ProcesoExtensiones.ProcesoMonitoreoExtensiones));
            workerThread.Start();
            ProcesoEventos.ProcesoMonitoreoEventos();
            workerThread.Join();
        }
    }
}
