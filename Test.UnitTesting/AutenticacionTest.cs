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
using System.IO;

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
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));     
                IMetodos logica = new Metodos();
                try
                {
                    logica.autenticarUsuario("usuario", "incorrecto");
                    Assert.Fail();
                }
                catch (InvalidCredentialsException e)
                {
                    Assert.IsTrue(true);
                }

                try
                {
                    //Assert.IsTrue(context.Users.FirstOrDefault(u => u.NombreUsuario == "usuario1") != null);
                    var result = logica.autenticarUsuario("usuario1", "usuario1");
                    Assert.IsNotNull(result);
                }
                catch (InvalidCredentialsException e)
                {
                    Assert.Fail();
                }
            }
        }
    }
}
