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

        // Este test esta fallando por como Identity hashea la pass al guardar el usuario
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

        /// <summary>
        /// Se prueba invocar al metodo registrarInicioUsuario() tanto con un 
        /// usuario existente como uno inexistente.
        /// </summary>
        [TestMethod]
        public void registrarInicioUsuarioTest()
        {
            using (var context = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();

                var user = new ApplicationUser() { UserName = "pruebaInicioUsuario", Nombre = "pruebaInicioUsuario", Contraseña = Contraseñas.GetSHA1("pruebaInicioUsuario") };
                context.Users.Add(user);

                // Se prueba con un usuario existente
                Assert.IsTrue(dbAL.registrarInicioUsuario("pruebaInicioUsuario", "tokenPrueba", DateTime.Now));

                // Se prueba con un usuario inexistente
                Assert.IsFalse(dbAL.registrarInicioUsuario("usuarioInexistente", "tokenPrueba", DateTime.Now));

                context.Users.Remove(user);
            }
        }

        /// <summary>
        /// Prueba el metodo corroborando que devuelva las zonas y recursos
        /// asociados al usuario
        /// </summary>
        [TestMethod]
        public void getRolUsuarioTest()
        {
            using (var context = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();

            }
        }

        /// <summary>
        /// Agrega un usuario con recursos y zonas asociados
        /// </summary>
        private void crearUsuario()
        {
            using (var context = new EmsysContext())
            {

            }
        }
    }
}
