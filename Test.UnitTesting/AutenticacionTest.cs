using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emsys.DataAccesLayer.Core;
using NUnit.Framework;
using Emsys.LogicLayer;
using Emsys.DataAccesLayer.Model;
using Emsys.LogicLayer.ApplicationExceptions;

namespace Test.UnitTesting
{
    [TestFixture]
    class AutenticacionTest
    {
        /// <summary>
        /// Agrega un usuario y luego ejecuta el metodo de autenticacion
        /// con credenciales validas y credenciales invalidas.
        /// </summary>
        [Test]
        public void AutenticarUsuarioTest()
        {
            using (var context = new EmsysContext())
            {
                IMetodos dbAL = new Metodos();

                var user = new ApplicationUser() { NombreUsuario = "pruebaAutenticar", Nombre = "pruebaAutenticar", Contraseña = "pruebaAutenticar" };
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
