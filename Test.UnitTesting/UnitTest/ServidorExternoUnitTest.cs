using NUnit.Framework;
using ServidorExterno;

namespace Test.UnitTesting
{
    [TestFixture]
    public class ServidorExternoUnitTest
    {
        /// <summary>
        /// Prueba servidor externo.
        /// </summary>
        [Test]
        public void ServicioExternoTest()
        {
            var serv = new ServidorExterno.Servicios();
            var resp = serv.Servicio1("uno", "dos", "tres");

            Assert.IsTrue(resp.Count == 2);
            Assert.IsTrue((resp[0].Count == 10) && (resp[1].Count == 10));
        }
        
    }
}
