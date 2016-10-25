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

namespace Test.UnitTesting
{
    [TestFixture]
    public class ObserverDataBaseUnitTest
    {
        private int _seconds = Convert.ToInt32(WebConfigurationManager.AppSettings["TiempoEsperaEnvioNotificaciones"]);

        /// <summary>
        /// prueba la logica de observer database
        /// </summary>
        [Test]
        public void PruebaOberverDatabase()
        {
            try
            {
                string[] entrada = new string[1];
                Thread workerThread = new Thread(new ThreadStart(SqlDependecyProject.Program.Main));
                workerThread.Start();
                //espero a que se disparen todos los hilos y luego empiezo a modificar la base
                Thread.Sleep(5000);
                ModificarBaseDatos();
                Thread.Sleep(30* _seconds * 1000+10000);
                workerThread.Abort();
                using (EmsysContext db= new EmsysContext()) {
                    Assert.IsTrue(db.LogNotification.Where(x=>x.EsError==false).Count()==30 );
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        //Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

           
        }

        private static void ModificarBaseDatos()
        {
            using (EmsysContext db = new EmsysContext())
            {

                foreach (var item in db.Evento)
                {
                    item.Descripcion = "otro";
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
                foreach (var item in db.Extensiones_Evento)
                {
                    item.DescripcionDespachador = "2016/07/23 21:30:00\\UsuarioDespachador\\descripcion de evento";
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
                //Thread.Sleep(10000);
                LogicLayerUnitTest test = new LogicLayerUnitTest();
                test.AdjuntarAudioTest();
                test.AdjuntarVideoTest();
                test.AdjuntarImagenTest();
                test.AdjuntarGeoUbicacion();

            }
        }
    }
}
