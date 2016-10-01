namespace Emsys.DataAccesLayer.Core
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public class Configuration : DbMigrationsConfiguration<EmsysContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EmsysContext context)
        {
            
         //   //Agregar unidades ejecutoras.
         //   Unidad_Ejecutora ue1 = new Unidad_Ejecutora() { Nombre = "ue1" };
         //   Unidad_Ejecutora ue2 = new Unidad_Ejecutora() { Nombre = "ue2" };
         //   Unidad_Ejecutora ue3 = new Unidad_Ejecutora() { Nombre = "ue3" };
         //   context.Unidades_Ejecutoras.Add(ue1);
         //   context.Unidades_Ejecutoras.Add(ue2);
         //   context.Unidades_Ejecutoras.Add(ue3);

         //   // Agregar zonas.
         //   Zona zona1 = new Zona() { Nombre = "zona1", Unidad_Ejecutora = ue1 };
         //   Zona zona2 = new Zona() { Nombre = "zona2", Unidad_Ejecutora = ue1 };
         //   Zona zona3 = new Zona() { Nombre = "zona3", Unidad_Ejecutora = ue2 };
         //   Zona zona4 = new Zona() { Nombre = "zona4", Unidad_Ejecutora = ue3 };
         //   context.Zonas.Add(zona1);
         //   context.Zonas.Add(zona2);
         //   context.Zonas.Add(zona3);
         //   context.Zonas.Add(zona4);

         //   // Agregar Sectores.
         //   Sector sector1 = new Sector() { Nombre = "sector1", Zona = zona1 };
         //   Sector sector2 = new Sector() { Nombre = "sector1", Zona = zona2 };
         //   Sector sector3 = new Sector() { Nombre = "sector1", Zona = zona3 };
         //   Sector sector4 = new Sector() { Nombre = "sector1", Zona = zona4 };
         //   context.Sectores.Add(sector1);
         //   context.Sectores.Add(sector2);
         //   context.Sectores.Add(sector3);
         //   context.Sectores.Add(sector4);

         //   // Agregar grupos recursos.
         //   Grupo_Recurso gr1 = new Grupo_Recurso() { Nombre = "gr1"};
         //   Grupo_Recurso gr2 = new Grupo_Recurso() { Nombre = "gr2" };
         //   context.Grupos_Recursos.Add(gr1);
         //   context.Grupos_Recursos.Add(gr2);

         //   // Agregar categorias.
         //   Categoria cat1 = new Categoria() { Codigo = "cod1", Clave = "Categoria de prueba 1", Prioridad = NombrePrioridad.Baja, Activo = true };
         //   Categoria cat2 = new Categoria() { Codigo = "cod2", Clave = "Categoria de prueba 2", Prioridad = NombrePrioridad.Media, Activo = true };
         //   Categoria cat3 = new Categoria() { Codigo = "cod3", Clave = "Categoria de prueba 3", Prioridad = NombrePrioridad.Alta, Activo = true };
         //   context.Categorias.Add(cat1);
         //   context.Categorias.Add(cat2);
         //   context.Categorias.Add(cat3);

         //   // Agregar recusos.
         //   Recurso rec1 = new Recurso() { Codigo = "recurso1", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>(), Extensiones_Eventos = new List<Extension_Evento>()};
         //   Recurso rec2 = new Recurso() { Codigo = "recurso2", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>(), Extensiones_Eventos = new List<Extension_Evento>()};
         //   Recurso rec3 = new Recurso() { Codigo = "recurso3", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>(), Extensiones_Eventos = new List<Extension_Evento>()};
         //   context.Recursos.Add(rec1);
         //   context.Recursos.Add(rec2);
         //   context.Recursos.Add(rec3);

         //   // Agregar recursos a grupos recursos.
         //   rec1.Grupos_Recursos.Add(gr1);
         //   rec2.Grupos_Recursos.Add(gr2);

         //   // Agregar rol.
         //   ApplicationRole rol1 = new ApplicationRole() { Nombre = "Admin", Permisos = new List<Permiso>()};
         //   context.ApplicationRoles.Add(rol1);

         //   // Agregar permisos.
         //   Permiso permiso1 = new Permiso() { Clave = "listarEventos", Descripcion = "Permite al usuario ver los eventos de sus zonas actuales/recurso" };
         //   context.Permisos.Add(permiso1);

         //   // Asignar permisos a roles.
         //   rol1.Permisos.Add(permiso1);

         //   // Agregar usuarios.
         //   var userStore = new UserStore<ApplicationUser>(context);
         //   var manager = new UserManager<ApplicationUser>(userStore);
         //   var user1 = new ApplicationUser() {NombreUsuario="A", Contraseña= "6dcd4ce23d88e2ee9568ba546c007c63d9131c1b", UserName = "usuario1", Nombre = "Usuario1" , ApplicationRoles = new List<ApplicationRole>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>(), Grupos_Recursos = new List<Grupo_Recurso>()};
         //   manager.Create(user1, "usuario1");
         //   var user2 = new ApplicationUser() { NombreUsuario = "B", Contraseña = "B" , UserName = "usuario2", Nombre = "Usuario2", ApplicationRoles = new List<ApplicationRole>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>(), Grupos_Recursos = new List<Grupo_Recurso>() };
         //   manager.Create(user2, "usuario2");

         //   // Asignar rol a usuarios.
         //   user1.ApplicationRoles.Add(rol1);
         //   user2.ApplicationRoles.Add(rol1);
         ////   user3.ApplicationRoles.Add(rol1);

         //   // Agregar usuarios a unidades ejecutoras y grupos recursos.
         //   user1.Grupos_Recursos.Add(gr1);
         //   user1.Unidades_Ejecutoras.Add(ue1);
         //   user1.Grupos_Recursos.Add(gr2);
         //   user1.Unidades_Ejecutoras.Add(ue2);
         //   user1.Unidades_Ejecutoras.Add(ue3);
         //   user2.Grupos_Recursos.Add(gr2);
         //   user2.Unidades_Ejecutoras.Add(ue2);
         //   //user3.Grupos_Recursos.Add(gr1);
         //   //user3.Unidades_Ejecutoras.Add(ue1);
            
         //   // Agregar eventos.
         //   Evento evento1 = new Evento()
         //   {
         //       Categoria = cat1,
         //       Estado = EstadoEvento.Enviado,
         //       Usuario = user1,
         //       Sector = sector1,
         //       EnProceso = true,
         //       TimeStamp = DateTime.Now,
         //       FechaCreacion = DateTime.Now
         //   };
         //   context.Evento.Add(evento1);
         //   Extension_Evento ext1 = new Extension_Evento() { Evento = evento1, Zona = zona1, Estado = EstadoExtension.FaltaDespachar, TimeStamp = DateTime.Now };
         //   context.Extensiones_Evento.Add(ext1);
            
         //   Evento evento2 = new Evento()
         //   {
         //       Categoria = cat2,
         //       Estado = EstadoEvento.Enviado,
         //       Usuario = user1,
         //       Sector = sector2,
         //       EnProceso = true,
         //       TimeStamp = DateTime.Now,
         //       FechaCreacion = DateTime.Now
         //   };
         //   context.Evento.Add(evento1);
         //   context.Extensiones_Evento.Add(new Extension_Evento() { Evento = evento2, Zona = zona2, Estado = EstadoExtension.FaltaDespachar, TimeStamp = DateTime.Now });
         //   context.Extensiones_Evento.Add(new Extension_Evento() { Evento = evento2, Zona = zona3, Estado = EstadoExtension.FaltaDespachar, TimeStamp = DateTime.Now });

         //   Evento evento3 = new Evento()
         //   {
         //       Categoria = cat3,
         //       Estado = EstadoEvento.Enviado,
         //       Usuario = user1,
         //       Sector = sector3,
         //       EnProceso = true,
         //       TimeStamp = DateTime.Now,
         //       FechaCreacion = DateTime.Now
         //   };
         //   context.Evento.Add(evento1);
         //   context.Extensiones_Evento.Add(new Extension_Evento() { Evento = evento3, Zona = zona4, Estado = EstadoExtension.FaltaDespachar, TimeStamp = DateTime.Now });

         //   // Asignar recursos a extensiones.
         //   rec1.Extensiones_Eventos.Add(ext1);
          
        }
    }
}
