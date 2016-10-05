using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using Microsoft.AspNet.Identity;
using Utils.Login;
using Emsys.LogicLayer;
using Emsys.LogicLayer.ApplicationExceptions;

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
                
                var user = new ApplicationUser() { NombreUsuario = "pruebaAutenticar", Nombre = "pruebaAutenticar", Contraseña = Contraseñas.GetSHA1("pruebaAutenticar") };
                context.Users.Add(user);

                // Pruebo con credenciales invalidas.
                try
                {
                    dbAL.autenticarUsuario("pruebaAutenticar", "pruebaIncorrecta");
                    Assert.Fail();
                }
                catch (InvalidCredentialsException e)
                {
                }
                catch (Exception e)
                {
                    Assert.Fail();
                }

                // Pruebo con credenciales invalidas.
                Assert.IsNotNull(dbAL.autenticarUsuario("pruebaAutenticar", "pruebaAutenticar"));
                
                context.Users.Remove(user);
            }
        }
    }
}
