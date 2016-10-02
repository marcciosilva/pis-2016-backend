using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    /// <summary>
    /// Summary description for AutenticacionTest
    /// </summary>
    [TestClass]
    public class AutenticacionTest
    {
        public AutenticacionTest()
        {
        }

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        /// Agrega un usuario y luego ejecuta iniciar sesion por recurso 
        /// con los datos del usuario agregado.
        /// </summary>
        [TestMethod]
        public void TestLoguearUsuarioRecurso()
        {

        }
    }
}
