using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emsys.LogicLayer;
using Emsys.DataAccesLayer.Core;

namespace Test.UnitTesting
{
    [TestFixture]
    class DALTest
    {
        ///// <summary>
        ///// Test que prueba que se pueda registrar un usuario a Identity.
        ///// </summary>
        //[Test]
        //public void RegistrarUsuarioTest()
        //{
        //    using (var db = new EmsysContext())
        //    {
        //        var user = new ApplicationUser() { NombreUsuario = "prueba", Nombre = "prueba" };
        //        db.Users.Add(user);
        //        db.SaveChanges();
        //        var usuarioAgregado = db.Users.FirstOrDefault(x => x.NombreUsuario.Equals("prueba"));
        //        Assert.IsNotNull(usuarioAgregado);
        //        db.Users.Remove(user);
        //    }
        //}

        ///// <summary>
        ///// Test que prueba que se pueda crear una Unidad Ejecutora y agregarla a la base de datos.
        ///// </summary>
        //[TestMethod]
        //public void CrearUnidadEjecutora()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        context.Unidades_Ejecutoras.Add(new Unidad_Ejecutora() { Nombre = "pruebaUE" });
        //        context.SaveChanges();
        //        var unidadEjecutoraAgregada = context.Unidades_Ejecutoras.FirstOrDefault(x => x.Nombre.Equals("pruebaUE"));
        //        Assert.IsNotNull(unidadEjecutoraAgregada);
        //        context.Unidades_Ejecutoras.Remove(unidadEjecutoraAgregada);
        //    }
        //}

        ///// <summary>
        ///// Test que prueba que se pueda crear una Zona y agregarla a la base de datos.
        ///// </summary>
        //[TestMethod]
        //public void CrearZona()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        context.Unidades_Ejecutoras.Add(new Unidad_Ejecutora() { Nombre = "pruebaUEZona" });
        //        context.SaveChanges();
        //        var unidadEjecutoraAgregada = context.Unidades_Ejecutoras.FirstOrDefault(x => x.Nombre.Equals("pruebaUEZona"));
        //        if (unidadEjecutoraAgregada != null)
        //        {
        //            context.Zonas.Add(new Zona() { Nombre = "pruebaZona", Unidad_Ejecutora = unidadEjecutoraAgregada });
        //            context.SaveChanges();
        //            var zonaAgregada = context.Zonas.FirstOrDefault(x => x.Nombre.Equals("pruebaZona"));
        //            Assert.IsNotNull(zonaAgregada);
        //            context.Zonas.Remove(zonaAgregada);
        //            context.Unidades_Ejecutoras.Remove(unidadEjecutoraAgregada);
        //        }
        //        else
        //        {
        //            Assert.Fail();
        //        }
        //    }
        //}

        ///// <summary>
        ///// Test que prueba que se pueda crear un Sector y agregarlo a la base de datos.
        ///// </summary>
        //[TestMethod]
        //public void CrearSector()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        context.Unidades_Ejecutoras.Add(new Unidad_Ejecutora() { Nombre = "pruebaUEZonaSector" });
        //        context.SaveChanges();
        //        var unidadEjecutoraAgregada = context.Unidades_Ejecutoras.FirstOrDefault(x => x.Nombre.Equals("pruebaUEZonaSector"));
        //        if (unidadEjecutoraAgregada != null)
        //        {
        //            context.Zonas.Add(new Zona() { Nombre = "pruebaZonaSector", Unidad_Ejecutora = unidadEjecutoraAgregada });
        //            context.SaveChanges();
        //            var zonaAgregada = context.Zonas.FirstOrDefault(x => x.Nombre.Equals("pruebaZonaSector"));
        //            if (zonaAgregada != null)
        //            {
        //                context.Sectores.Add(new Sector() { Nombre = "prueba", Zona = zonaAgregada });
        //                context.SaveChanges();
        //                var sectorAgregado = context.Sectores.FirstOrDefault(x => x.Nombre.Equals("prueba"));
        //                Assert.IsNotNull(sectorAgregado);
        //                context.Sectores.Remove(sectorAgregado);
        //                context.Zonas.Remove(zonaAgregada);
        //                context.Unidades_Ejecutoras.Remove(unidadEjecutoraAgregada);
        //            }
        //            else
        //            {
        //                Assert.Fail();
        //            }

        //        }
        //        else
        //        {
        //            Assert.Fail();
        //        }
        //    }
        //}

        //static void CrearCategoria()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Codigo de la categoria:");
        //            string codigo = Console.ReadLine();

        //            Console.WriteLine("Descripcion de la categoria:");
        //            string clave = Console.ReadLine();

        //            Console.WriteLine("Prioridad de la categoria (0-> Alta, 1-> Media, 2-> Baja): ");
        //            int prioridad = Int32.Parse(Console.ReadLine());

        //            context.Categorias.Add(new Categoria() { Codigo = codigo, Clave = clave, Prioridad = (NombrePrioridad)prioridad, Activo = true });
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void CrearEvento()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Id de la Categoria:");
        //            int categoria = Int32.Parse(Console.ReadLine());

        //            Console.WriteLine("Estado (Creado->0, Enviado->1):");
        //            int estado = Int32.Parse(Console.ReadLine());

        //            Console.WriteLine("Nombre de usuario del creador:");
        //            string creador = Console.ReadLine();

        //            Console.WriteLine("Id del sector de origen:");
        //            int sector = Int32.Parse(Console.ReadLine());

        //            Console.WriteLine("En proceso (0 , 1):");
        //            int enProceso = Int32.Parse(Console.ReadLine());

        //            Evento ev = new Evento()
        //            {
        //                Categoria = context.Categorias.Find(categoria),
        //                Estado = (EstadoEvento)estado,
        //                Usuario = context.Users.FirstOrDefault(u => u.UserName == creador),
        //                Sector = context.Sectores.Find(sector),
        //                EnProceso = enProceso != 0,
        //                TimeStamp = DateTime.Now,
        //                FechaCreacion = DateTime.Now
        //            };

        //            List<int> zonas = new List<int>();
        //            bool seguir = true;
        //            while (seguir)
        //            {
        //                Console.WriteLine("Id de una zona del evento:");
        //                zonas.Add(Int32.Parse(Console.ReadLine()));
        //                Console.WriteLine("1-> Agregar mas zonas, 0-> No agregar mas zonas");
        //                seguir = Int32.Parse(Console.ReadLine()) == 1;
        //            }
        //            foreach (int z in zonas)
        //            {
        //                context.Extensiones_Evento.Add(new Extension_Evento() { Evento = ev, Zona = context.Zonas.Find(z), Estado = EstadoExtension.FaltaDespachar, TimeStamp = DateTime.Now });
        //            }
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void CrearRecurso()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Codigo:");
        //            string codigo = Console.ReadLine();

        //            Recurso rec = new Recurso() { Codigo = codigo, Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>() };
        //            context.Recursos.Add(rec);

        //            List<int> grupos = new List<int>();
        //            bool seguir = true;
        //            while (seguir)
        //            {
        //                Console.WriteLine("Id de un grupo recurso del recurso:");
        //                grupos.Add(Int32.Parse(Console.ReadLine()));
        //                Console.WriteLine("1-> Agregar mas grupos, 0-> No agregar mas gripos");
        //                seguir = Int32.Parse(Console.ReadLine()) == 1;
        //            }
        //            foreach (int g in grupos)
        //            {
        //                rec.Grupos_Recursos.Add(context.Grupos_Recursos.Find(g));
        //            }

        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void AsignarRecurso()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Codigo del recurso a asignar:");
        //            string codigo = Console.ReadLine();

        //            Console.WriteLine("Id de la extension a la cual asignar el recurso:");
        //            int extension = Int32.Parse(Console.ReadLine());

        //            context.Recursos.FirstOrDefault(r => r.Codigo == codigo).Extensiones_Eventos.Add(context.Extensiones_Evento.Find(extension));
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void AgregarClaim()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Nombre de usuario:");
        //            string usuario = Console.ReadLine();

        //            Console.WriteLine("ClaimType:");
        //            string type = Console.ReadLine();

        //            Console.WriteLine("ClaimValue:");
        //            string value = Console.ReadLine();

        //            context.Users.FirstOrDefault(u => u.UserName == usuario).Claims.Add(new IdentityUserClaim() { ClaimType = type, ClaimValue = value });
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void IniciarUsuarioPorRecurso()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Nombre de usuario:");
        //            string usuario = Console.ReadLine();

        //            Console.WriteLine("Id del recurso:");
        //            int recurso = Int32.Parse(Console.ReadLine());

        //            context.Recursos.Find(recurso).Usuario = context.Users.FirstOrDefault(us => us.UserName == usuario);
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void CrearGrupoRecurso()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Nombre de grupo:");
        //            string nombre = Console.ReadLine();

        //            context.Grupos_Recursos.Add(new Grupo_Recurso() { Nombre = nombre });
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void AgregarRol()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Nombre de rol:");
        //            string rol = Console.ReadLine();

        //            context.ApplicationRoles.Add(new ApplicationRole() { Nombre = rol });
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void AgregarPermiso()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Clave del permiso:");
        //            string clave = Console.ReadLine();

        //            Console.WriteLine("Descripcion del permiso:");
        //            string descripcion = Console.ReadLine();

        //            context.Permisos.Add(new Permiso() { Clave = clave, Descripcion = descripcion });
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void AsignarPermiso()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Nombre del permiso:");
        //            string permiso = Console.ReadLine();

        //            Console.WriteLine("Nombre del rol:");
        //            string rol = Console.ReadLine();

        //            context.ApplicationRoles.FirstOrDefault(r => r.Nombre == rol).Permisos.Add(context.Permisos.FirstOrDefault(per => per.Clave == permiso));
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void AsignarRol()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Nombre de usuario:");
        //            string usuario = Console.ReadLine();

        //            Console.WriteLine("Nombre del rol:");
        //            string rol = Console.ReadLine();

        //            context.Users.FirstOrDefault(u => u.UserName == usuario).ApplicationRoles.Add(context.ApplicationRoles.FirstOrDefault(r => r.Nombre == rol));
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void AsignarGR()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Nombre de usuario:");
        //            string usuario = Console.ReadLine();

        //            Console.WriteLine("Id del grupo recurso:");
        //            int gr = Int32.Parse(Console.ReadLine());

        //            context.Users.FirstOrDefault(u => u.UserName == usuario).Grupos_Recursos.Add(context.Grupos_Recursos.Find(gr));
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}

        //static void AsignarUE()
        //{
        //    using (var context = new EmsysContext())
        //    {
        //        try
        //        {
        //            Console.WriteLine("Nombre de usuario:");
        //            string usuario = Console.ReadLine();

        //            Console.WriteLine("Id de la unidad ejecutora:");
        //            int ue = Int32.Parse(Console.ReadLine());

        //            context.Users.FirstOrDefault(u => u.UserName == usuario).Unidades_Ejecutoras.Add(context.Unidades_Ejecutoras.Find(ue));
        //            context.SaveChanges();
        //            Console.WriteLine("Exito!");
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //    }
        //}
    }
}
