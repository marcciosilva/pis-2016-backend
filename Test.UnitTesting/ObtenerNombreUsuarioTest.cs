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
    class ObtenerNombreUsuarioTest
    {
        /// <summary>
        /// Agrega un usuario y luego ejecuta el metodo de autenticacion
        /// con credenciales validas y credenciales invalidas.
        /// </summary>
        [Test]
        public void GetNombreUsuarioTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));       

                IMetodos logica = new Metodos();
                var resp = logica.autenticarUsuario("usuario1", "usuario1");
                var token = resp.access_token;
                Assert.IsTrue(logica.getNombreUsuario(token) == "usuario1");                
            }
        }
    }
}
