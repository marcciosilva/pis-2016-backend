namespace Emsys.DataAccesLayer.Core
{
    using System.Data.Entity.Migrations;

    public class Configuration : DbMigrationsConfiguration<EmsysContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        /// <summary>
        /// Este metodo sirve para ingresar contenido a la base de datos.
        /// </summary>
        /// <param name="context">Contecto a la base de datos.</param>
        protected override void Seed(EmsysContext context)
        {
            ////try
            ////{
            ////    //Agregar unidades ejecutoras.
            ////    Unidad_Ejecutora ue1 = new Unidad_Ejecutora() { Nombre = "ue1" };
            ////    Unidad_Ejecutora ue2 = new Unidad_Ejecutora() { Nombre = "ue2" };
            ////    Unidad_Ejecutora ue3 = new Unidad_Ejecutora() { Nombre = "ue3" };
            ////    context.Unidades_Ejecutoras.Add(ue1);
            ////    context.Unidades_Ejecutoras.Add(ue2);
            ////    context.Unidades_Ejecutoras.Add(ue3);

            //    // Agregar zonas.
            //    Zona zona1 = new Zona() { Nombre = "zona1", UnidadEjecutora = ue1 };
            //    Zona zona2 = new Zona() { Nombre = "zona2", UnidadEjecutora = ue1 };
            //    Zona zona3 = new Zona() { Nombre = "zona3", UnidadEjecutora = ue2 };
            //    Zona zona4 = new Zona() { Nombre = "zona4", UnidadEjecutora = ue3 };
            //    context.Zonas.Add(zona1);
            //    context.Zonas.Add(zona2);
            //    context.Zonas.Add(zona3);
            //    context.Zonas.Add(zona4);

            ////    // Agregar Sectores.
            ////    Sector sector1 = new Sector() { Nombre = "sector1", Zona = zona1 };
            ////    Sector sector2 = new Sector() { Nombre = "sector1", Zona = zona2 };
            ////    Sector sector3 = new Sector() { Nombre = "sector1", Zona = zona3 };
            ////    Sector sector4 = new Sector() { Nombre = "sector1", Zona = zona4 };
            ////    context.Sectores.Add(sector1);
            ////    context.Sectores.Add(sector2);
            ////    context.Sectores.Add(sector3);
            ////    context.Sectores.Add(sector4);

            ////    // Agregar grupos recursos.
            ////    Grupo_Recurso gr1 = new Grupo_Recurso() { Nombre = "gr1" };
            ////    Grupo_Recurso gr2 = new Grupo_Recurso() { Nombre = "gr2" };
            ////    context.Grupos_Recursos.Add(gr1);
            ////    context.Grupos_Recursos.Add(gr2);

            ////    // Agregar categorias.
            ////    Categoria cat1 = new Categoria() { Codigo = "cod1", Clave = "Categoria de prueba 1", Prioridad = NombrePrioridad.Baja, Activo = true };
            ////    Categoria cat2 = new Categoria() { Codigo = "cod2", Clave = "Categoria de prueba 2", Prioridad = NombrePrioridad.Media, Activo = true };
            ////    Categoria cat3 = new Categoria() { Codigo = "cod3", Clave = "Categoria de prueba 3", Prioridad = NombrePrioridad.Alta, Activo = true };
            ////    context.Categorias.Add(cat1);
            ////    context.Categorias.Add(cat2);
            ////    context.Categorias.Add(cat3);

            ////    // Agregar recusos.
            ////    Recurso rec1 = new Recurso() { Codigo = "recurso1", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>(), Extensiones_Eventos = new List<Extension_Evento>() };
            ////    Recurso rec2 = new Recurso() { Codigo = "recurso2", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>(), Extensiones_Eventos = new List<Extension_Evento>() };
            ////    Recurso rec3 = new Recurso() { Codigo = "recurso3", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, Grupos_Recursos = new List<Grupo_Recurso>(), Extensiones_Eventos = new List<Extension_Evento>() };
            ////    context.Recursos.Add(rec1);
            ////    context.Recursos.Add(rec2);
            ////    context.Recursos.Add(rec3);

            ////    // Agregar recursos a grupos recursos.
            ////    rec1.Grupos_Recursos.Add(gr1);
            ////    rec2.Grupos_Recursos.Add(gr2);

            //    // Agregar rol.
            //    Rol rol1 = new Rol() { Nombre = "Admin", Permisos = new List<Permiso>() };
            //    context.ApplicationRoles.Add(rol1);

            //    // Agregar permisos.
            //    Permiso permiso1 = new Permiso() { Clave = "listarEventos", Descripcion = "Permite al usuario ver los eventos de sus zonas actuales/recurso" };
            //    context.Permisos.Add(permiso1);
            //    Permiso permiso2 = new Permiso() { Clave = "adjuntarMultimedia", Descripcion = "Permite al usuario adjuntar multimedia" };
            //    context.Permisos.Add(permiso2);

            //    // Asignar permisos a roles.
            //    rol1.Permisos.Add(permiso1);
            //    rol1.Permisos.Add(permiso2);

            //    // Agregar usuarios.
            //    var user1 = new Usuario() { Id = 1, NombreLogin = "A", Contraseña = "6dcd4ce23d88e2ee9568ba546c007c63d9131c1b", Nombre = "Usuario1", ApplicationRoles = new List<Rol>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>(), Grupos_Recursos = new List<Grupo_Recurso>() };
            //    var user2 = new Usuario() { Id = 2, NombreLogin = "B", Contraseña = "B", Nombre = "Usuario2", ApplicationRoles = new List<Rol>(), Unidades_Ejecutoras = new List<Unidad_Ejecutora>(), Grupos_Recursos = new List<Grupo_Recurso>() };
            //    context.Users.AddOrUpdate(x => x.Id, user1, user2);
            //    context.Users.AddOrUpdate(x => x.Id, user1, user2);

            //    // Asignar rol a usuarios.
            //    user1.ApplicationRoles.Add(rol1);
            //    user2.ApplicationRoles.Add(rol1);

            ////    // Agregar usuarios a unidades ejecutoras y grupos recursos.
            ////    user1.Grupos_Recursos.Add(gr1);
            ////    user1.Unidades_Ejecutoras.Add(ue1);
            ////    user1.Grupos_Recursos.Add(gr2);
            ////    user1.Unidades_Ejecutoras.Add(ue2);
            ////    user1.Unidades_Ejecutoras.Add(ue3);
            ////    user2.Grupos_Recursos.Add(gr2);
            ////    user2.Unidades_Ejecutoras.Add(ue2);
            ////    //user3.Grupos_Recursos.Add(gr1);
            ////    //user3.Unidades_Ejecutoras.Add(ue1);

            //    //    // Agregar eventos.
            //    Evento evento1 = new Evento()
            //    {
            //        Categoria = cat1,
            //        Estado = EstadoEvento.Enviado,
            //        Usuario = user1,
            //        Sector = sector1,
            //        Descripcion = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años, sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum.",
            //        EnProceso = true,
            //        TimeStamp = DateTime.Now,
            //        FechaCreacion = DateTime.Now,
            //        Latitud = 19.9,
            //        Longitud = 20.5,
            //        GeoUbicaciones = new List<GeoUbicacion>()
            //    };
            //    context.Evento.Add(evento1);
            //    Extension_Evento ext1 = new Extension_Evento()
            //    {
            //        DescripcionDespachador = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años, sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum.",
            //        Evento = evento1,
            //        Zona = zona1,
            //        Estado = EstadoExtension.FaltaDespachar,
            //        TimeStamp = DateTime.Now,
            //        GeoUbicaciones = new List<GeoUbicacion>()
            //    };
            //    context.Extensiones_Evento.Add(ext1);

            ////    Evento evento2 = new Evento()
            ////    {
            ////        Categoria = cat2,
            ////        Estado = EstadoEvento.Enviado,
            ////        Usuario = user1,
            ////        Sector = sector2,
            ////        EnProceso = true,
            ////        TimeStamp = DateTime.Now,
            ////        FechaCreacion = DateTime.Now
            ////    };
            ////    context.Evento.Add(evento1);
            ////    context.Extensiones_Evento.Add(new Extension_Evento()
            ////    {
            ////        DescripcionDespachador = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años, sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum.",
            ////        Evento = evento2,
            ////        Zona = zona2,
            ////        Estado = EstadoExtension.FaltaDespachar,
            ////        TimeStamp = DateTime.Now
            ////    });
            ////    context.Extensiones_Evento.Add(new Extension_Evento()
            ////    {
            ////        DescripcionDespachador = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años, sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum.",
            ////        Evento = evento2,
            ////        Zona = zona3,
            ////        Estado = EstadoExtension.FaltaDespachar,
            ////        TimeStamp = DateTime.Now
            ////    });

            ////    Evento evento3 = new Evento()
            ////    {
            ////        Categoria = cat3,
            ////        Estado = EstadoEvento.Enviado,
            ////        Usuario = user1,

            ////        Sector = sector3,
            ////        EnProceso = true,
            ////        TimeStamp = DateTime.Now,
            ////        FechaCreacion = DateTime.Now
            ////    };
            ////    context.Evento.Add(evento1);
            ////    context.Extensiones_Evento.Add(new Extension_Evento()
            ////    {
            ////        DescripcionDespachador = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen. No sólo sobrevivió 500 años, sino que tambien ingresó como texto de relleno en documentos electrónicos, quedando esencialmente igual al original. Fue popularizado en los 60s con la creación de las hojas las cuales contenian pasajes de Lorem Ipsum, y más recientemente con software de autoedición, como por ejemplo Aldus PageMaker, el cual incluye versiones de Lorem Ipsum.",
            ////        Evento = evento3,
            ////        Zona = zona4,
            ////        Estado = EstadoExtension.FaltaDespachar,
            ////        TimeStamp = DateTime.Now
            ////    });

            ////    // Asignar recursos a extensiones.
            ////    rec1.Extensiones_Eventos.Add(ext1);

            ////    // Agregar geo ubicaciones.
            ////    GeoUbicacion geo1 = new GeoUbicacion() { Latitud = 11.1, Longitud = 22.2, FechaEnvio = DateTime.Now, Usuario = user1 };
            ////    GeoUbicacion geo2 = new GeoUbicacion() { Latitud = 33.3, Longitud = 44.4, FechaEnvio = DateTime.Now, Usuario = user1 };
            ////    GeoUbicacion geo3 = new GeoUbicacion() { Latitud = 55.5, Longitud = 66.6, FechaEnvio = DateTime.Now, Usuario = user1 };
            ////    GeoUbicacion geo4 = new GeoUbicacion() { Latitud = 77.7, Longitud = 88.8, FechaEnvio = DateTime.Now, Usuario = user1 };

            ////    context.GeoUbicaciones.Add(geo1);
            ////    context.GeoUbicaciones.Add(geo1);
            ////    context.GeoUbicaciones.Add(geo3);
            ////    context.GeoUbicaciones.Add(geo4);

            ////    // Agregar geo ubicaciones a eventos y extensiones.
            ////    evento1.GeoUbicaciones.Add(geo1);
            ////    evento1.GeoUbicaciones.Add(geo2);
            ////    ext1.GeoUbicaciones.Add(geo3);
            ////    ext1.GeoUbicaciones.Add(geo4);
            ////}
            ////catch (Exception e)
            ////{


            ////}
        }
    }
}
