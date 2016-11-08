using System;
using System.Linq;
using Emsys.DataAccesLayer.Core;
using NUnit.Framework;
using System.IO;
using Emsys.LogicLayer;
using Utils.Notifications;
using System.Data.Entity.Validation;
using System.Threading;
using System.Web.Configuration;
using Emsys.DataAccesLayer.Model;
using System.Collections.Generic;

namespace Test.UnitTesting
{
    //descomentar esto si falla
    //[TestFixture]
    public class ObserverDataBaseUnitTestStress
    {
        private int _seconds = Convert.ToInt32(WebConfigurationManager.AppSettings["TiempoEsperaEnvioNotificaciones"]);

        /// <summary>
        /// prueba la logica de observer database
        /// </summary>
        //[Test]
        public void ObserverDataBaseTestStress()
        {
            try
            {
                string[] entrada = new string[1];
                Thread workerThread = new Thread(new ThreadStart(SqlDependecyProject.Program.Main));
                workerThread.Start();

                Thread workerThreadAnalysisDataNotifications = new Thread(new ThreadStart(HiloDeScreenShoots));
                workerThreadAnalysisDataNotifications.Start();
                //espero a que se disparen todos los hilos y luego empiezo a modificar la base
                Thread.Sleep(5000);

                EmsysContext db = new EmsysContext();

                for (int i = 0; i < 500; i++)
                {
                    Modificaciones(db);
                    Thread.Sleep(3000);
                }


                // me quedo colgado por que sino mato todo los threads y no puedo ver si el test fue exitoso.
                Thread.Sleep(240 * _seconds * 1000 + 10000);
                workerThread.Abort();
            }
            catch (Exception)
            {
                using (EmsysContext db = new EmsysContext())
                {
                    var cantidadEnviosReales = db.LogNotification.Where(x => x.Codigo == 901).Count();
                    var cantidadEnviosExitosos = db.LogNotification.Where(x => x.Codigo == 906).Count();
                    var cantidadEnviosError = db.LogNotification.Where(x => x.Codigo == 904).Count();
                    Assert.IsTrue(cantidadEnviosReales == cantidadEnviosExitosos);
                }
            }
            using (EmsysContext db = new EmsysContext())
            {
                var cantidadEnviosReales = db.LogNotification.Where(x => x.Codigo == 901).Count();
                var cantidadEnviosExitosos = db.LogNotification.Where(x => x.Codigo == 906).Count();
                var cantidadEnviosError = db.LogNotification.Where(x => x.Codigo == 904).Count();
                Assert.IsTrue(cantidadEnviosReales == cantidadEnviosExitosos);
            }
        }

        private void HiloDeScreenShoots()
        {
            while (true)
            {
                // Un minuto.
                Thread.Sleep(60000);
                Util.NotificacionAnalysis.Program.CapturoDatos();
            }
        }

        private static int ModificarBaseDatos()
        {
            using (EmsysContext db = new EmsysContext())
            {
                int contador = Modificaciones(db);

                AsignacionRecurso ar = new AsignacionRecurso
                {
                    ActualmenteAsignado = true,
                    AsignacionRecursoDescripcion = new List<AsignacionRecursoDescripcion>(),
                    Descripcion = "",
                    Extension = db.ExtensionesEvento.FirstOrDefault(),
                    HoraArribo = DateTime.Now,
                    Recurso = db.Recursos.FirstOrDefault(),
                };
                db.AsignacionesRecursos.Add(ar);
                db.SaveChanges();
                ar.Descripcion = "nueva";
                db.SaveChanges();

                ////Thread.Sleep(10000);
                LogicLayerUnitTest test = new LogicLayerUnitTest();
                test.AdjuntarAudioTest();//  1
                test.AdjuntarVideoTest();
                test.AdjuntarImagenTest();
                test.AdjuntarGeoUbicacion();
                test.ActualizarDescripcionRecursoTest();
                return contador;
            }
        }

        private static int Modificaciones(EmsysContext db)
        {
            var contador = 0;
            foreach (var item in db.Evento)
            {
                item.Descripcion = DateTime.Now.ToString();
                item.Categoria = new Emsys.DataAccesLayer.Model.Categoria
                {

                    Activo = true,
                    Clave = "key",
                    Codigo = "cod",
                    Prioridad = new Emsys.DataAccesLayer.Model.NombrePrioridad(),
                };
                item.Sector = new Emsys.DataAccesLayer.Model.Sector
                {
                    Nombre = "sector",
                };
            }
            db.SaveChanges();
            foreach (var item in db.ExtensionesEvento)
            {
                item.DescripcionDespachador = DateTime.Now.ToString();
            }
            db.SaveChanges();
            foreach (var item in db.Videos)
            {
                item.FechaEnvio = DateTime.Now;
            }
            db.SaveChanges();
            foreach (var item in db.Audios)
            {
                item.FechaEnvio = DateTime.Now;
            }
            db.SaveChanges();
            foreach (var item in db.GeoUbicaciones)
            {
                item.FechaEnvio = DateTime.Now;
            }
            db.SaveChanges();
            foreach (var item in db.AsignacionRecursoDescripcion)
            {
                item.Fecha = DateTime.Now;
            }
            db.SaveChanges();
            return contador;
        }
    }
}
