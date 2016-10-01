using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emsys.Logs;
using Emsys.DataAccesLayer.Core;
using NUnit.Framework;
using System.IO;

namespace Test.UnitTesting
{
    [TestFixture]
    public class LogsUnitTest
    {
        [Test]
        public void AgregarLog()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            var cantidadLogsPrevia = db.Logs.Count()+1;
            int PruebaConstante = 12345678;
            string nombre = Guid.NewGuid().ToString();
            Log.AgregarLog(nombre, "1:1:1:1", "PruebaUnitaria", "LogUnitTest", 1, "agregar log", "esto es una prueba", PruebaConstante);
            var cantidadLogsDespues = db.Logs.Count();
            Assert.True(cantidadLogsPrevia==cantidadLogsDespues);
            var log= db.Logs.Where(x => x.Usuario == nombre).FirstOrDefault();
            Assert.NotNull(log);

        }

        [Test]
        public void AgregarLogError()
        {
            AppDomain.CurrentDomain.SetData(
            "DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            EmsysContext db = new EmsysContext();

            var cantidadLogsPrevia = db.Logs.Count() + 1;
            int PruebaConstante = 12345678;
            string nombre = Guid.NewGuid().ToString();
            Log.AgregarLogError(nombre, "1:1:1:1", "PruebaUnitaria", "LogUnitTest", 1, "agregar log", "esto es una prueba", PruebaConstante);
            var cantidadLogsDespues = db.Logs.Count();
            Assert.True(cantidadLogsPrevia == cantidadLogsDespues);
            var log = db.Logs.Where(x => x.Usuario == nombre).FirstOrDefault();
            Assert.NotNull(log);
        }
    }
}
