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
        /// <summary>
        /// Prueba enviar notificaciones
        /// </summary>
        [Test]
        public void PruebaNotificar()
        {
            INotifications manejadorNotificaciones = FactoryNotifications.GetInstance();
            manejadorNotificaciones.SendMessage("Esto es una prueba", 1, 1 ,1, "zona1", "Prueba");
            Assert.IsTrue(true);
        }
        
    }
}
