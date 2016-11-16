namespace SqlDependecyProject
{
    using System;
    using System.Linq;
    using System.Threading;
    using Emsys.DataAccesLayer.Core;
    using Emsys.LogicLayer;
    using DataTypeObject;

    public class Program
    {
        /// <summary>
        /// Metodo principal para el proyecto ObserverDatabase encargado de manejar las notificaciones de los cambios de la bd.
        /// </summary>
        public static void Main()
        {
            try
            {
                // Para iniciar la bd si no esta creada.
                using (EmsysContext db = new EmsysContext())
                {
                    db.Evento.ToList();
                }
                // Para cada tabla a ser monitoreada inicio un thread.
                Thread WorkerThreadExtensiones = new Thread(new ThreadStart(ProcesoExtensiones.ProcesoMonitoreoExtensiones));
                WorkerThreadExtensiones.IsBackground = true;
                WorkerThreadExtensiones.Start();


                Thread WorkerThreadVideos = new Thread(new ThreadStart(ProcesoVideos.ProcesoMonitoreoVideos));
                WorkerThreadExtensiones.IsBackground = true;
                WorkerThreadVideos.Start();

                Thread WorkerThreadAudios = new Thread(new ThreadStart(ProcesoAudios.ProcesoMonitoreoAudios));
                WorkerThreadExtensiones.IsBackground = true;
                WorkerThreadAudios.Start();

                Thread WorkerThreadGeoUbicacuibes = new Thread(new ThreadStart(ProcesoGeoUbicacion.ProcesoMonitoreoGeoUbicaciones));
                WorkerThreadExtensiones.IsBackground = true;
                WorkerThreadGeoUbicacuibes.Start();

                Thread WorkerThreadAsignacionRecursoDescripcion = new Thread(new ThreadStart(ProcesoAsignacionRecursoDescripcion.ProcesoMonitorearAsignacionRecursoDescripcion));
                WorkerThreadExtensiones.IsBackground = true;
                WorkerThreadAsignacionRecursoDescripcion.Start();


                Thread WorkerThreadImagenes = new Thread(new ThreadStart(ProcesoImagenes.ProcesoMonitoreoImagenes));
                WorkerThreadExtensiones.IsBackground = true;
                WorkerThreadImagenes.Start();

                Thread WorkerThreadAsignacionRecurso = new Thread(new ThreadStart(ProcesoAsignacionRecurso.ProcesoAsignacionRecursoMonitoreo));
                WorkerThreadExtensiones.IsBackground = true;
                WorkerThreadAsignacionRecurso.Start();

                Thread workerEventos = new Thread(new ThreadStart(ProcesoEventos.ProcesoMonitoreoEventos));
                workerEventos.IsBackground = true;
                workerEventos.Start();
            }
            catch (Exception e)
            {
                IMetodos dbAL = new Metodos();
                dbAL.AgregarLogError("vacio", "servidor", "ObserverDataBase", "No hay entidad expecifica", 0, "Program", "Error generico en observerDatabase: " + e.ToString(), MensajesParaFE.LogErrorObserverDataBaseeGenerico);
            }
        }
    }
}