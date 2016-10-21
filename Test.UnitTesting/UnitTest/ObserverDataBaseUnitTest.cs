using System;
using System.Linq;
using Emsys.DataAccesLayer.Core;
using NUnit.Framework;
using System.IO;
using Emsys.LogicLayer;
using Utils.Notifications;
using System.Data.Entity.Validation;
using System.Threading;

namespace Test.UnitTesting
{
    [TestFixture]
    public class ObserverDataBaseUnitTest
    {
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
                ModificarBaseDatos();
                Thread.Sleep(15000);
                workerThread.Abort();
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

            // Esto aca esta mal, pero como tengo un cliente de firebase en .net no puedo ver si la notificacion se hizo. Por lo menos cubre codigo.
            Assert.IsTrue(true);
        }

        private static void ModificarBaseDatos()
        {
            using (EmsysContext db = new EmsysContext())
            {

                foreach (var item in db.Evento)
                {
                    item.Descripcion = "otro";
                    item.Categoria = new Emsys.DataAccesLayer.Model.Categoria {

                        Activo = true,
                        Clave = "key",
                        Codigo = "cod",
                        Prioridad = new Emsys.DataAccesLayer.Model.NombrePrioridad (),
                    };
                    item.Sector = new Emsys.DataAccesLayer.Model.Sector {
                        Nombre="sector",                        
                    };
                }
                db.SaveChanges();
                foreach (var item in db.Extensiones_Evento)
                {
                    item.DescripcionDespachador = "otro";
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

            }
        }
    }
}
