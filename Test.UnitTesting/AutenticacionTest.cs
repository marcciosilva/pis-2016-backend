using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emsys.DataAccesLayer.Core;
using NUnit.Framework;
using Emsys.LogicLayer;
using Emsys.LogicLayer.Utils;
using Emsys.LogicLayer.ApplicationExceptions;
using System.IO;
using Emsys.DataAccesLayer.Model;
using DataTypeObject;
using System.Data.SqlClient;

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
                string pass = Passwords.GetSHA1("usuario1");
                ApplicationUser usuarioPrueba = new ApplicationUser { NombreUsuario = "usuario1", Contraseña = pass };
                context.Users.Add(usuarioPrueba);
                context.SaveChanges();
                IMetodos logica = new Metodos();
                try
                {
                    logica.autenticarUsuario("usuario1", "incorrecto");
                    Assert.Fail();
                }
                catch (InvalidCredentialsException e)
                {
                    Assert.IsTrue(true);
                }

                try
                {
                    var result = logica.autenticarUsuario("usuario1", "usuario1");
                    Assert.IsNotNull(result);
                }
                catch (InvalidCredentialsException e)
                {
                    Assert.Fail();
                }
            }
        }

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

                var rol = new ApplicationRole() { Nombre = "pruebaRol", Permisos = new List<Permiso>(), Usuarios = new List<ApplicationUser>() };
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

        [Test]
        public void loguearUsuarioRecursoTest()
        {
            using (var context = new EmsysContext())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));

                // Se crea un usuario con un recurso asociado en la BD.
                var user = new ApplicationUser() { NombreUsuario = "usuarioPruebaRecurso", Nombre = "usuarioPruebaRecurso", Contraseña = Passwords.GetSHA1("usuarioPruebaRecurso"), Grupos_Recursos = new List<Grupo_Recurso>() };
                var user2 = new ApplicationUser() { NombreUsuario = "usuarioPruebaRecursoNoDisponible", Nombre = "usuarioPruebaRecursoNoDisponible", Contraseña = Passwords.GetSHA1("usuarioPruebaRecursoNoDisponible"), Grupos_Recursos = new List<Grupo_Recurso>() };
                var recursoDisponible = new Recurso() { Codigo = "recursoPruebaDisponible", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre };
                var recursoNoSeleccionable = new Recurso() { Codigo = "recursoNoSeleccionable", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre };
                var gr = new Grupo_Recurso() { Nombre = "grPrueba", Recursos = new List<Recurso>() };
                gr.Recursos.Add(recursoDisponible);
                user.Grupos_Recursos.Add(gr);
                user2.Grupos_Recursos.Add(gr);
                context.Recursos.Add(recursoDisponible);
                context.Recursos.Add(recursoNoSeleccionable);
                context.Grupos_Recursos.Add(gr);
                context.Users.Add(user);
                context.Users.Add(user2);
                context.SaveChanges();

                IMetodos logica = new Metodos();

                // Obtengo token de usuario. 
                var autent = logica.autenticarUsuario("usuarioPruebaRecurso", "usuarioPruebaRecurso");
                string token = autent.access_token;

                // Recurso seleccionable por el usuario y disponible.
                List<DtoRecurso> lRecurso = new List<DtoRecurso>();
                DtoRecurso dtoRecurso = new DtoRecurso() { id = recursoDisponible.Id, codigo = "recursoPruebaDisponible" };
                lRecurso.Add(dtoRecurso);
                DtoRol rol = new DtoRol() { recursos = lRecurso, zonas = new List<DtoZona>() };

                try
                {
                    Assert.IsTrue(logica.loguearUsuario(token, rol));    
                }
                catch(RecursoNoDisponibleException e)
                {
                    Assert.Fail();
                }
               
                // Pruebo loguearme con otro usuario que tenga el mismo recurso asignado
                var autent2 = logica.autenticarUsuario("usuarioPruebaRecursoNoDisponible", "usuarioPruebaRecursoNoDisponible");
                string token2 = autent.access_token;
                try
                {
                    logica.loguearUsuario(token, rol);
                    Assert.Fail();
                }
                catch (RecursoNoDisponibleException e)
                {
                }

                // Recurso no seleccionable por el usuario.
                List<DtoRecurso> lRecurso2 = new List<DtoRecurso>();
                DtoRecurso dtoRecurso2 = new DtoRecurso() { id = recursoNoSeleccionable.Id, codigo = "recursoNoSeleccionable" };
                lRecurso2.Add(dtoRecurso2);
                DtoRol rol2 = new DtoRol() { recursos = lRecurso2, zonas = new List<DtoZona>() };
                try
                {
                    Assert.IsFalse(logica.loguearUsuario(token2, rol2));
                }
                catch (RecursoNoDisponibleException e)
                {
                    Assert.Fail();
                }
            }
        }


        /// <summary>
        /// Se llama luego de correr cada test y borra la base de datos.
        /// </summary>
        [SetUp]
        public void limpiarBase()
        {
            using (var context = new EmsysContext())
            {
                foreach(var u in context.Evento)
                {
                    context.Evento.Remove(u);
                }

                foreach (var u in context.Users)
                {
                    context.Users.Remove(u);
                }
                foreach (var gr in context.Grupos_Recursos)
                {
                    context.Grupos_Recursos.Remove(gr);
                }
                foreach (var r in context.Recursos)
                {
                    context.Recursos.Remove(r);
                }
                context.SaveChanges();
            }
        }
    }
}
