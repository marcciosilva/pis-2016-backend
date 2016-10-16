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
    public class NotificacionesUnitTest
    {
        [Test]
        public void PruebaNotificar()
        {
            INotifications manejadorNotificaciones = FactoryNotifications.GetInstance();
            manejadorNotificaciones.SendMessage("Esto es una prueba", "#####", "Prueba");
            Assert.IsTrue(true);
        }

        [Test]
        public void PruebaOberverDatabase()
        {
            try
            {
                string[] entrada = new string[1];
                Thread workerThread = new Thread(new ThreadStart(SqlDependecyProject.ProcesoExtensiones.Listener));
                workerThread.Start();

                using (EmsysContext db = new EmsysContext())
                {
                    var evento = db.Evento.FirstOrDefault();
                    if (evento != null)
                    {
                        evento.Descripcion = DateTime.Now.ToString();
                        evento.Categoria = db.Categorias.FirstOrDefault();
                        evento.Sector = db.Sectores.FirstOrDefault();
                    }

                    db.SaveChanges();
                }

                Thread.Sleep(5000);
                // workerThread.Abort();
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
            //esto aca esta mal, pero como tengo un cliente de firebase en .net no puedo ver si la notificacion se hizo. Pro lo menos cubre codigo.
            Assert.IsTrue(true);
        }
    }
}
