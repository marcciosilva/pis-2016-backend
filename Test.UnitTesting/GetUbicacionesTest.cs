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
    class GetUbicacionesTest
    {
        /// <summary>
        /// Agrega un usuario y luego ejecuta el metodo de autenticacion
        /// con credenciales validas y credenciales invalidas.
        /// </summary>
        [Test]
        public void AutorizarUsuarioTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

                var user = new ApplicationUser() { NombreUsuario = "usuario", Nombre = "pepe", Contraseña = Emsys.LogicLayer.Utils.Passwords.GetSHA1("pruebapass") };
                context.Users.Add(user);

                var permiso = new Permiso() { Clave = "pruebaPermiso", Roles = new List<ApplicationRole>() };
                context.Permisos.Add(permiso);

                var rol = new ApplicationRole() { Nombre = "pruebaRol" , Permisos = new List<Permiso>(), Usuarios = new List<ApplicationUser>()};
                context.ApplicationRoles.Add(rol);

                rol.Permisos.Add(permiso);
                rol.Usuarios.Add(user);

                IMetodos logica = new Metodos();

                context.SaveChanges();
                
                var autent = logica.autenticarUsuario("usuario", "pruebapass");
                string token = autent.access_token;

                string[] etiqueta1 = { "permisoFalso" };
                Assert.IsFalse(logica.autorizarUsuario(token, etiqueta1));

                string[] etiqueta2 = { "pruebaPermiso" };
                Assert.IsTrue(logica.autorizarUsuario(token, etiqueta2));
                
            }
        }
    }
}
