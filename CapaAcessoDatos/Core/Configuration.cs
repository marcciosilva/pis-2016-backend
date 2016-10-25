namespace Emsys.DataAccesLayer.Core
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public class Configuration : DbMigrationsConfiguration<EmsysContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            //AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// Este metodo sirve para ingresar contenido a la base de datos.
        /// </summary>
        /// <param name="context">Contecto a la base de datos.</param>
        protected override void Seed(EmsysContext context)
        {
            try
            {
                // Agregar unidades ejecutoras.
                Unidad_Ejecutora ue1 = new Unidad_Ejecutora() { Id = 1, Nombre = "ue1" };
                Unidad_Ejecutora ue2 = new Unidad_Ejecutora() { Id = 2, Nombre = "ue2" };
                Unidad_Ejecutora ue3 = new Unidad_Ejecutora() { Id = 3, Nombre = "ue3" };

                // No va por que se agrega luego.
                //context.Unidades_Ejecutoras.AddOrUpdate(x => x.Id, ue1);
                //context.Unidades_Ejecutoras.AddOrUpdate(x => x.Id, ue2);
                //context.Unidades_Ejecutoras.AddOrUpdate(x => x.Id, ue3);

                // Agregar zonas.
                Zona zona1 = new Zona() { Id = 1, Nombre = "zona1", UnidadEjecutora = ue1 };
                Zona zona2 = new Zona() { Id = 2, Nombre = "zona2", UnidadEjecutora = ue1 };
                Zona zona3 = new Zona() { Id = 3, Nombre = "zona3", UnidadEjecutora = ue2 };
                Zona zona4 = new Zona() { Id = 4, Nombre = "zona4", UnidadEjecutora = ue3 };

                // Se agregan luego.
                //context.Zonas.AddOrUpdate(y => y.Id, zona1);
                //context.Zonas.AddOrUpdate(y => y.Id, zona2);
                //context.Zonas.AddOrUpdate(y => y.Id, zona3);
                //context.Zonas.AddOrUpdate(y => y.Id, zona4);

                // Agregar Sectores.
                Sector sector1 = new Sector() { Id = 1, Nombre = "sector1", Zona = zona1 };
                Sector sector2 = new Sector() { Id = 2, Nombre = "sector2", Zona = zona2 };
                Sector sector3 = new Sector() { Id = 3, Nombre = "sector3", Zona = zona3 };
                Sector sector4 = new Sector() { Id = 4, Nombre = "sector4", Zona = zona4 };

                // Se agregan luego.
                //context.Sectores.AddOrUpdate(m => m.Id, sector1);
                //context.Sectores.AddOrUpdate(m => m.Id, sector2);
                //context.Sectores.AddOrUpdate(m => m.Id, sector3);
                //context.Sectores.AddOrUpdate(m => m.Id, sector4);

                // Agregar grupos recursos.
                Grupo_Recurso gr1 = new Grupo_Recurso() { Id = 1, Nombre = "gr1" };
                Grupo_Recurso gr2 = new Grupo_Recurso() { Id = 2, Nombre = "gr2" };

                // Agregar categorias.
                Categoria cat1 = new Categoria() { Id = 1, Codigo = "cod1", Clave = "Categoria de prueba 1", Prioridad = NombrePrioridad.Baja, Activo = true };
                Categoria cat2 = new Categoria() { Id = 2, Codigo = "cod2", Clave = "Categoria de prueba 2", Prioridad = NombrePrioridad.Media, Activo = true };
                Categoria cat3 = new Categoria() { Id = 3, Codigo = "cod3", Clave = "Categoria de prueba 3", Prioridad = NombrePrioridad.Alta, Activo = true };
                //se agregan despues
                //context.Categorias.AddOrUpdate(m => m.Id, cat1);
                //context.Categorias.AddOrUpdate(m => m.Id, cat2);
                //context.Categorias.AddOrUpdate(m => m.Id, cat3);

                // Agregar recusos.
                Recurso rec1 = new Recurso() { Id = 1, Codigo = "recurso1", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>(), Extensiones_Eventos = new List<Extension_Evento>() };
                Recurso rec2 = new Recurso() { Id = 2, Codigo = "recurso2", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>(), Extensiones_Eventos = new List<Extension_Evento>() };
                Recurso rec3 = new Recurso() { Id = 3, Codigo = "recurso3", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>(), Extensiones_Eventos = new List<Extension_Evento>() };

                // Agregar recursos a grupos recursos. No se agregan directo.
                rec1.Grupos_Recursos.Add(gr1);
                rec2.Grupos_Recursos.Add(gr2);

                context.Recursos.AddOrUpdate(m => m.Id, rec1);
                context.Recursos.AddOrUpdate(m => m.Id, rec2);
                context.Recursos.AddOrUpdate(m => m.Id, rec3);

                // Agregar rol.
                Rol rol1 = new Rol() { Id = 1, Nombre = "Admin", Permisos = new List<Permiso>() };

                // Agregar permisos.
                Permiso permiso1 = new Permiso() { Id = 1, Nombre = "listarEventos", Clave = "listarEventos", Descripcion = "Permite al usuario ver los eventos de sus zonas actuales/recurso" };

                Permiso permiso2 = new Permiso() { Id = 2, Nombre = "adjuntarMultimedia", Clave = "adjuntarMultimedia", Descripcion = "Permite al usuario adjuntar multimedia" };
                //no va
                //context.Permisos.AddOrUpdate(x => x.Id, permiso2);
                //context.Permisos.AddOrUpdate(x => x.Id, permiso1);

                // Asignar permisos a roles.
                rol1.Permisos.Add(permiso1);
                rol1.Permisos.Add(permiso2);

                //context.ApplicationRoles.AddOrUpdate(x => x.Id, rol1);

                // Agregar usuarios.
                var user1 = new Usuario() { Id = 1, NombreLogin = "A", Contraseña = "6dcd4ce23d88e2ee9568ba546c007c63d9131c1b", Nombre = "Usuario1", ApplicationRoles = new List<Rol>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>(), Grupos_Recursos = new List<Grupo_Recurso>() };
                var user2 = new Usuario() { Id = 2, NombreLogin = "B", Contraseña = "ae4f281df5a5d0ff3cad6371f76d5c29b6d953ec", Nombre = "Usuario2", ApplicationRoles = new List<Rol>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>(), Grupos_Recursos = new List<Grupo_Recurso>() };
                var user3 = new Usuario() { Id = 3, NombreLogin = "C", Contraseña = "32096c2e0eff33d844ee6d675407ace18289357d", Nombre = "Usuario3", ApplicationRoles = new List<Rol>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>(), Grupos_Recursos = new List<Grupo_Recurso>() };
                var user4 = new Usuario() { Id = 4, NombreLogin = "D", Contraseña = "50c9e8d5fc98727b4bbc93cf5d64a68db647f04f", Nombre = "Usuario4", ApplicationRoles = new List<Rol>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>(), Grupos_Recursos = new List<Grupo_Recurso>() };

                // Se agregan luego.
                context.Users.AddOrUpdate(x => x.Id, user1);
                context.Users.AddOrUpdate(x => x.Id, user2);
                context.Users.AddOrUpdate(x => x.Id, user3);
                context.Users.AddOrUpdate(x => x.Id, user4);

                // Asignar rol a usuarios.
                user1.ApplicationRoles.Add(rol1);
                user2.ApplicationRoles.Add(rol1);
                user3.ApplicationRoles.Add(rol1);
                user4.ApplicationRoles.Add(rol1);

                // Agregar usuarios a unidades ejecutoras y grupos recursos.
                user1.Grupos_Recursos.Add(gr1);
                user1.Grupos_Recursos.Add(gr2);
                user2.Grupos_Recursos.Add(gr2);
                user3.Grupos_Recursos.Add(gr2);
                user4.Grupos_Recursos.Add(gr1);

                user1.Unidades_Ejecutoras.Add(ue1);
                user1.Unidades_Ejecutoras.Add(ue2);
                user1.Unidades_Ejecutoras.Add(ue3);
                user2.Unidades_Ejecutoras.Add(ue2);
                user3.Unidades_Ejecutoras.Add(ue3);
                user4.Unidades_Ejecutoras.Add(ue1);
                user4.Unidades_Ejecutoras.Add(ue2);

                // Origen eventos.
                //Origen_Evento oe1 = new Origen_Evento()
                //{
                //    Id = 1,
                //    TimeStamp = DateTime.Now,
                //    TipoOrigen = "test",
                //    IdOrigen = 1
                //};

                // Agregar eventos.
                Evento evento1 = new Evento()
                {
                    Id = 1,
                    Categoria = cat1,
                    Estado = EstadoEvento.Enviado,
                    Usuario = user1,
                    Sector = sector1,
                    Descripcion = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas semper mollis velit et euismod. Donec et fringilla risus. Praesent fermentum mi sit amet dui varius, et hendrerit ex congue. Cras consectetur vel ex sit amet consectetur. Curabitur ac tincidunt nisi, in dignissim dui. Fusce placerat arcu at leo dictum, ac rutrum massa ullamcorper. Proin ut ex est. Curabitur lectus libero, facilisis vitae lacus eget, auctor pulvinar ex. Vestibulum in ligula scelerisque, viverra purus sed, consectetur dui. Aenean ut nisl tellus. Aliquam at metus dolor. In eros ligula, accumsan ac dui eu, efficitur bibendum risus. Sed quam metus, malesuada quis placerat a, maximus vitae ipsum.",
                    EnProceso = true,
                    TimeStamp = DateTime.Now,
                    FechaCreacion = DateTime.Now,
                    Latitud = 19.9,
                    Longitud = 20.5,
                    GeoUbicaciones = new List<GeoUbicacion>(),
                    //Origen_Evento = oe1
                };

                Evento evento2 = new Evento()
                {
                    Id = 2,
                    Categoria = cat2,
                    Estado = EstadoEvento.Enviado,
                    Usuario = user1,
                    Sector = sector2,
                    EnProceso = true,
                    TimeStamp = DateTime.Now,
                    FechaCreacion = DateTime.Now
                };

                Evento evento3 = new Evento()
                {
                    Id = 3,
                    Categoria = cat3,
                    Estado = EstadoEvento.Enviado,
                    Usuario = user1,
                    Sector = sector3,
                    EnProceso = true,
                    TimeStamp = DateTime.Now,
                    FechaCreacion = DateTime.Now
                };
                //context.Evento.AddOrUpdate(x => x.Id, evento1);
                //context.Evento.AddOrUpdate(x => x.Id, evento2);
                //context.Evento.AddOrUpdate(x => x.Id, evento3);
                Extension_Evento ext1 = new Extension_Evento()
                {
                    Id = 1,
                    DescripcionDespachador = "2016/07/23 21:30:00\\UsuarioDespachador\\descripcion de evento\\2016/07/23 21:37:00\\UsuarioDespachador2\\otra descripcion de evento\\2016/07/24 10:37:00\\UsuarioDespachador2\\otra mas",
                    Evento = evento1,
                    Zona = zona1,
                    Estado = EstadoExtension.FaltaDespachar,
                    TimeStamp = DateTime.Now,
                    GeoUbicaciones = new List<GeoUbicacion>(),
                    Recursos = new List<Recurso>(),
                    AsignacionesRecursos = new List<AsignacionRecurso>(),
                };
                ext1.AsignacionesRecursos.Add(new AsignacionRecurso
                {
                    ActualmenteAsignado = true,
                    AsignacionRecursoDescripcion = new List<AsignacionRecursoDescripcion>(),
                    Extension = ext1,
                    Recurso = rec1,
                    HoraArribo = null,
                    Descripcion = string.Empty
                });
                Extension_Evento ext2 = new Extension_Evento()
                {
                    Id = 2,
                    DescripcionDespachador = "2016/07/23 21:30:00\\UsuarioDespachador\\descripcion de evento\\2016/07/23 21:37:00\\UsuarioDespachador2\\otra descripcion de evento\\2016/07/24 10:37:00\\UsuarioDespachador2\\otra mas",
                    Evento = evento2,
                    Zona = zona2,
                    Estado = EstadoExtension.FaltaDespachar,
                    TimeStamp = DateTime.Now
                };
                Extension_Evento ext3 = new Extension_Evento()
                {
                    Id = 3,
                    DescripcionDespachador = "2016/07/23 21:30:00\\UsuarioDespachador\\descripcion de evento\\2016/07/23 21:37:00\\UsuarioDespachador2\\otra descripcion de evento\\2016/07/24 10:37:00\\UsuarioDespachador2\\otra mas",
                    Evento = evento3,
                    Zona = zona4,
                    Estado = EstadoExtension.FaltaDespachar,
                    TimeStamp = DateTime.Now
                };

                // Agregar geo ubicaciones.
                GeoUbicacion geo1 = new GeoUbicacion() { Latitud = 11.1, Longitud = 22.2, FechaEnvio = DateTime.Now, Usuario = user1 };
                GeoUbicacion geo2 = new GeoUbicacion() { Latitud = 33.3, Longitud = 44.4, FechaEnvio = DateTime.Now, Usuario = user1 };
                GeoUbicacion geo3 = new GeoUbicacion() { Latitud = 55.5, Longitud = 66.6, FechaEnvio = DateTime.Now, Usuario = user1 };
                GeoUbicacion geo4 = new GeoUbicacion() { Latitud = 77.7, Longitud = 88.8, FechaEnvio = DateTime.Now, Usuario = user1 };

                // Agregar geo ubicaciones a eventos y extensiones.
                evento1.GeoUbicaciones.Add(geo1);
                evento1.GeoUbicaciones.Add(geo2);
                ext1.GeoUbicaciones.Add(geo3);
                ext1.GeoUbicaciones.Add(geo4);

                ext1.Recursos.Add(rec1);

                context.Extensiones_Evento.AddOrUpdate(x => x.Id, ext1);
                context.Extensiones_Evento.AddOrUpdate(x => x.Id, ext2);
                context.Extensiones_Evento.AddOrUpdate(x => x.Id, ext3);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
