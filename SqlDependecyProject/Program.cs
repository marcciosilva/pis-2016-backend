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
        public static void Main()
        {
            try
            {
              
                // Para cada tabla a ser monitoreada inicio un thread.
                Thread WorkerThreadExtensiones = new Thread(new ThreadStart(ProcesoExtensiones.ProcesoMonitoreoExtensiones));
                WorkerThreadExtensiones.Start();

                Thread WorkerThreadVideos = new Thread(new ThreadStart(ProcesoVideos.ProcesoMonitoreoVideos));
                WorkerThreadVideos.Start();

                Thread WorkerThreadAudios = new Thread(new ThreadStart(ProcesoAudios.ProcesoMonitoreoAudios));
                WorkerThreadAudios.Start();

                Thread WorkerThreadAsignacionRecursoDescripcion = new Thread(new ThreadStart(ProcesoAsignacionRecursoDescripcion.ProcesoMonitorearAsignacionRecursoDescripcion));
                WorkerThreadAsignacionRecursoDescripcion.Start();                
                
                Thread WorkerThreadImagenes = new Thread(new ThreadStart(ProcesoImagenes.ProcesoMonitoreoImagenes));
                WorkerThreadImagenes.Start();

                Thread WorkerThreadAsignacionRecurso = new Thread(new ThreadStart(ProcesoAsignacionRecurso.ProcesoAsignacionRecursoMonitoreo));
                WorkerThreadAsignacionRecurso.Start();


                ProcesoEventos.ProcesoMonitoreoEventos();

                WorkerThreadExtensiones.Join();
                WorkerThreadVideos.Join();
                WorkerThreadAudios.Join();
                WorkerThreadAsignacionRecursoDescripcion.Join();
                WorkerThreadImagenes.Join();
                WorkerThreadAsignacionRecurso.Join();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
        }
    }
}
