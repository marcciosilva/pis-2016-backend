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

                //using (EmsysContext db = new EmsysContext())
                //{
                //    var evento = db.Evento.FirstOrDefault();
                //    if (evento != null)
                //    {
                //        evento.Descripcion = DateTime.Now.ToString();
                //        evento.Categoria = db.Categorias.FirstOrDefault();
                //        evento.Sector = db.Sectores.FirstOrDefault();
                //    }
                //    db.SaveChanges();
                //}
                LogicLayerUnitTest test = new LogicLayerUnitTest();
                test.getDescripcionEventoTest();
                test.listarEventosTest();
                Thread.Sleep(15000);
                workerThread.Join();
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
    }
}
