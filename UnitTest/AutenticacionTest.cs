using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using Microsoft.AspNet.Identity;
using CapaAcessoDatos;
using Utils.Login;

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
        /// Agrega un usuario y luego ejecuta el metodo de autenticacion
        /// con credenciales validas y credenciales invalidas.
        /// </summary>
        [TestMethod]
        public void AutenticarUsuarioTest()
        {
            using (var context = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();
                
                var user = new ApplicationUser() { UserName = "pruebaAutenticar", Nombre = "pruebaAutenticar", Contraseña = Contraseñas.GetSHA1("pruebaAutenticar") };
                context.Users.Add(user);

                // Pruebo con credenciales invalidas.
                Assert.IsFalse(dbAL.autenticarUsuario("pruebaAutenticar", Contraseñas.GetSHA1("pruebaIncorrecta")));

                // Pruebo con credenciales invalidas.
                Assert.IsTrue(dbAL.autenticarUsuario("pruebaAutenticar", Contraseñas.GetSHA1("pruebaAutenticar")));
                
                context.Users.Remove(user);
            }
        }
    }
}
