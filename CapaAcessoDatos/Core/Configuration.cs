namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using Model;

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
                // Agregar departamentos.
                Departamento dep1 = new Departamento() { Id = 1, Nombre = "departamento1" };
                Departamento dep2 = new Departamento() { Id = 2, Nombre = "departamento2" };
                Departamento dep3 = new Departamento() { Id = 3, Nombre = "departamento3" };
                Departamento dep4 = new Departamento() { Id = 4, Nombre = "departamento4" };

                context.Departamentos.AddOrUpdate(dep1);
                context.Departamentos.AddOrUpdate(dep2);
                context.Departamentos.AddOrUpdate(dep3);
                context.Departamentos.AddOrUpdate(dep4);

                // Agregar unidades ejecutoras.
                UnidadEjecutora ue1 = new UnidadEjecutora() { Id = 1, Nombre = "ue1" };
                UnidadEjecutora ue2 = new UnidadEjecutora() { Id = 2, Nombre = "ue2" };
                UnidadEjecutora ue3 = new UnidadEjecutora() { Id = 3, Nombre = "ue3" };
                

                // Agregar zonas.
                Zona zona1 = new Zona() { Id = 1, Nombre = "zona1", UnidadEjecutora = ue1, Sectores = new List<Sector>() };
                Zona zona2 = new Zona() { Id = 2, Nombre = "zona2", UnidadEjecutora = ue1, Sectores = new List<Sector>() };
                Zona zona3 = new Zona() { Id = 3, Nombre = "zona3", UnidadEjecutora = ue2, Sectores = new List<Sector>() };
                Zona zona4 = new Zona() { Id = 4, Nombre = "zona4", UnidadEjecutora = ue3, Sectores = new List<Sector>() };
                

                // Agregar Sectores.
                Sector sector1 = new Sector() { Id = 1, Nombre = "sector1", Zona = zona1 };
                Sector sector2 = new Sector() { Id = 2, Nombre = "sector2", Zona = zona2 };
                Sector sector3 = new Sector() { Id = 3, Nombre = "sector3", Zona = zona3 };
                Sector sector4 = new Sector() { Id = 4, Nombre = "sector4", Zona = zona4 };

                // Sectores a zonas.
                zona1.Sectores.Add(sector1);
                zona2.Sectores.Add(sector2);
                zona3.Sectores.Add(sector3);
                zona4.Sectores.Add(sector4);

                // Agregar grupos recursos.
                GrupoRecurso gr1 = new GrupoRecurso() { Id = 1, Nombre = "gr1" };
                GrupoRecurso gr2 = new GrupoRecurso() { Id = 2, Nombre = "gr2" };

                // Agregar categorias.
                Categoria cat1 = new Categoria() { Id = 1, Codigo = "cod1", Clave = "Categoria de prueba 1", Prioridad = NombrePrioridad.Baja, Activo = true };
                Categoria cat2 = new Categoria() { Id = 2, Codigo = "cod2", Clave = "Categoria de prueba 2", Prioridad = NombrePrioridad.Media, Activo = true };
                Categoria cat3 = new Categoria() { Id = 3, Codigo = "cod3", Clave = "Categoria de prueba 3", Prioridad = NombrePrioridad.Alta, Activo = true };
                
                // Agregar recusos.
                Recurso rec1 = new Recurso() { Id = 1, Codigo = "recurso1", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, GruposRecursos = new List<GrupoRecurso>(), ExtensionesEventos = new List<ExtensionEvento>() };
                Recurso rec2 = new Recurso() { Id = 2, Codigo = "recurso2", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, GruposRecursos = new List<GrupoRecurso>(), ExtensionesEventos = new List<ExtensionEvento>() };
                Recurso rec3 = new Recurso() { Id = 3, Codigo = "recurso3", Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre, GruposRecursos = new List<GrupoRecurso>(), ExtensionesEventos = new List<ExtensionEvento>() };

                // Agregar recursos a grupos recursos. No se agregan directo.
                rec1.GruposRecursos.Add(gr1);
                rec2.GruposRecursos.Add(gr2);

                context.Recursos.AddOrUpdate(m => m.Id, rec1);
                context.Recursos.AddOrUpdate(m => m.Id, rec2);
                context.Recursos.AddOrUpdate(m => m.Id, rec3);

                // Agregar rol.
                Rol rol1 = new Rol() { Id = 1, Nombre = "Admin", Permisos = new List<Permiso>() };
                Rol rol2 = new Rol() { Id = 2, Nombre = "Recurso", Permisos = new List<Permiso>() };
                Rol rol3 = new Rol() { Id = 3, Nombre = "Despachador", Permisos = new List<Permiso>() };

                // Agregar permisos.
                Permiso permiso1 = new Permiso() { Id = 1, Nombre = "Listar eventos", Clave = "listarEventos", Descripcion = "Permite al usuario ver la lista de eventos de sus zonas actuales/recurso actual." };
                Permiso permiso2 = new Permiso() { Id = 2, Nombre = "Adjuntar multimedia", Clave = "adjuntarMultimedia", Descripcion = "Permite al usuario adjuntar multimedia." };
                Permiso permiso3 = new Permiso() { Id = 3, Nombre = "Login", Clave = "login", Descripcion = "Permite al usuario iniciar sesion, mantenerse conectado y cerrar sesion." };
                Permiso permiso4 = new Permiso() { Id = 4, Nombre = "Consumir servicio externo", Clave = "consumirServicioExterno", Descripcion = "Permite al usuario consumir un servicio externo." };
                Permiso permiso5 = new Permiso() { Id = 5, Nombre = "Obtener evento", Clave = "obtenerEvento", Descripcion = "Permite al usuario consultar la informacion de eventos." };
                Permiso permiso6 = new Permiso() { Id = 6, Nombre = "Actualizar descripcion (Recurso)", Clave = "actualizarDescripcionRecurso", Descripcion = "Permite a un usuario recurso actualizar la descripcion de extensiones de evento." };
                Permiso permiso7 = new Permiso() { Id = 7, Nombre = "Reportar hora de arribo", Clave = "reportarHoraArribo", Descripcion = "Permite a un usuario recurso reportar la hora de arribo a eventos." };
                Permiso permiso8 = new Permiso() { Id = 8, Nombre = "Obtener informacion para creacion de eventos", Clave = "infoCreacionEvento", Descripcion = "Permite a un usuario obtener informacion necesaria para crear eventos." };
                Permiso permiso9 = new Permiso() { Id = 9, Nombre = "Crear Evento", Clave = "crearEvento", Descripcion = "Permite a un usuario crear eventos." };
                Permiso permiso10 = new Permiso() { Id = 10, Nombre = "Despachar extension", Clave = "despacharExtension", Descripcion = "Permite a un usuario tomar y liberar extensiones para despacharlas." };
                Permiso permiso11 = new Permiso() { Id = 11, Nombre = "Gestionar recursos de extension", Clave = "gestionarRecursosExtension", Descripcion = "Permite a un despachador asignar o quitar recursos de una extension de evento." };
                Permiso permiso12 = new Permiso() { Id = 12, Nombre = "Actualizar segunda categoria", Clave = "actualizarSegundaCategoria", Descripcion = "Permite a un despachador actualizar la segunda categoria de una extension de evento." };
                Permiso permiso13 = new Permiso() { Id = 13, Nombre = "Abrir extension", Clave = "abrirExtension", Descripcion = "Permite a un despachador abrir una extension nueva para un evento." };
                Permiso permiso14 = new Permiso() { Id = 14, Nombre = "Cerrar extension", Clave = "cerrarExtension", Descripcion = "Permite a un despachador pasar a estado 'cerrado' la extension de un evento." };
                Permiso permiso15 = new Permiso() { Id = 15, Nombre = "Actualizar descripcion (Despachador)", Clave = "actualizarDescripcionDespachador", Descripcion = "Permite a un despachador actualizar la descripcion de una extension de evento." };
                Permiso permiso16 = new Permiso() { Id = 16, Nombre = "Adjuntar geo ubicacion", Clave = "adjuntarGeoUbicacion", Descripcion = "Permite a un usuario adjuntar geo ubicaciones a extensiones de evento." };
                Permiso permiso17 = new Permiso() { Id = 17, Nombre = "Ver multimedia", Clave = "verMultimedia", Descripcion = "Permite a un usuario ver informacion multimedia." };

                // Asignar permisos a roles.
                // Permisos admin (totales).
                rol1.Permisos.Add(permiso1);
                rol1.Permisos.Add(permiso2);
                rol1.Permisos.Add(permiso3);
                rol1.Permisos.Add(permiso4);
                rol1.Permisos.Add(permiso5);
                rol1.Permisos.Add(permiso6);
                rol1.Permisos.Add(permiso7);
                rol1.Permisos.Add(permiso8);
                rol1.Permisos.Add(permiso9);
                rol1.Permisos.Add(permiso10);
                rol1.Permisos.Add(permiso11);
                rol1.Permisos.Add(permiso12);
                rol1.Permisos.Add(permiso13);
                rol1.Permisos.Add(permiso14);
                rol1.Permisos.Add(permiso15);
                rol1.Permisos.Add(permiso16);
                rol1.Permisos.Add(permiso17);

                // Permisos recurso.
                rol2.Permisos.Add(permiso1);
                rol2.Permisos.Add(permiso2);
                rol2.Permisos.Add(permiso3);
                rol2.Permisos.Add(permiso4);
                rol2.Permisos.Add(permiso5);
                rol2.Permisos.Add(permiso6);
                rol2.Permisos.Add(permiso7);
                rol2.Permisos.Add(permiso8);
                rol2.Permisos.Add(permiso9);
                rol2.Permisos.Add(permiso16);
                rol2.Permisos.Add(permiso17);

                // Permisos despachador (totales).
                rol3.Permisos.Add(permiso1);
                rol3.Permisos.Add(permiso2);
                rol3.Permisos.Add(permiso3);
                rol3.Permisos.Add(permiso4);
                rol3.Permisos.Add(permiso5);
                rol3.Permisos.Add(permiso8);
                rol3.Permisos.Add(permiso9);
                rol3.Permisos.Add(permiso10);
                rol3.Permisos.Add(permiso11);
                rol3.Permisos.Add(permiso12);
                rol3.Permisos.Add(permiso13);
                rol3.Permisos.Add(permiso14);
                rol3.Permisos.Add(permiso15);
                rol3.Permisos.Add(permiso16);
                rol3.Permisos.Add(permiso17);
                
                // Agregar usuarios.
                var user1 = new Usuario() { Id = 1, NombreLogin = "A", Contraseña = "6dcd4ce23d88e2ee9568ba546c007c63d9131c1b", Nombre = "Usuario1", ApplicationRoles = new List<Rol>(), UnidadesEjecutoras = new List<UnidadEjecutora>(), GruposRecursos = new List<GrupoRecurso>() };
                var user2 = new Usuario() { Id = 2, NombreLogin = "B", Contraseña = "ae4f281df5a5d0ff3cad6371f76d5c29b6d953ec", Nombre = "Usuario2", ApplicationRoles = new List<Rol>(), UnidadesEjecutoras = new List<UnidadEjecutora>(), GruposRecursos = new List<GrupoRecurso>() };
                var user3 = new Usuario() { Id = 3, NombreLogin = "C", Contraseña = "32096c2e0eff33d844ee6d675407ace18289357d", Nombre = "Usuario3", ApplicationRoles = new List<Rol>(), UnidadesEjecutoras = new List<UnidadEjecutora>(), GruposRecursos = new List<GrupoRecurso>() };
                var user4 = new Usuario() { Id = 4, NombreLogin = "D", Contraseña = "50c9e8d5fc98727b4bbc93cf5d64a68db647f04f", Nombre = "Usuario4", ApplicationRoles = new List<Rol>(), UnidadesEjecutoras = new List<UnidadEjecutora>(), GruposRecursos = new List<GrupoRecurso>() };

                // Se agregan luego.
                context.Usuarios.AddOrUpdate(x => x.Id, user1);
                context.Usuarios.AddOrUpdate(x => x.Id, user2);
                context.Usuarios.AddOrUpdate(x => x.Id, user3);
                context.Usuarios.AddOrUpdate(x => x.Id, user4);

                // Asignar rol a usuarios.
                user1.ApplicationRoles.Add(rol1);
                user2.ApplicationRoles.Add(rol1);
                user3.ApplicationRoles.Add(rol1);
                user4.ApplicationRoles.Add(rol1);

                // Agregar usuarios a unidades ejecutoras y grupos recursos.
                user1.GruposRecursos.Add(gr1);
                user1.GruposRecursos.Add(gr2);
                user2.GruposRecursos.Add(gr2);
                user3.GruposRecursos.Add(gr2);
                user4.GruposRecursos.Add(gr1);

                user1.UnidadesEjecutoras.Add(ue1);
                user1.UnidadesEjecutoras.Add(ue2);
                user1.UnidadesEjecutoras.Add(ue3);
                user2.UnidadesEjecutoras.Add(ue2);
                user3.UnidadesEjecutoras.Add(ue3);
                user4.UnidadesEjecutoras.Add(ue1);
                user4.UnidadesEjecutoras.Add(ue2);

                
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
                    Departamento = dep1
                };

                // Origen eventos.
                OrigenEvento oe1 = new OrigenEvento()
                {
                    Id = 1,
                    TimeStamp = DateTime.Now,
                    TipoOrigen = "test",
                    IdOrigen = 1,
                    Evento = evento1
                };
                evento1.OrigenEvento = oe1;

                Evento evento2 = new Evento()
                {
                    Id = 2,
                    Categoria = cat2,
                    Estado = EstadoEvento.Enviado,
                    Usuario = user1,
                    Sector = sector2,
                    EnProceso = true,
                    TimeStamp = DateTime.Now,
                    Departamento = dep2,
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
                    Departamento = dep3,
                    FechaCreacion = DateTime.Now
                };

                string formato = "yyyy-MM-dd'T'hh:mm:ss.FFF";
                ExtensionEvento ext1 = new ExtensionEvento()
                {
                    Id = 1,
                    DescripcionDespachador = DateTime.Now.ToString(formato) + "\\UsuarioDespachador\\descripcion de evento\\" + DateTime.Now.ToString(formato) + "\\UsuarioDespachador2\\otra descripcion de evento\\" + DateTime.Now.ToString(formato) + "\\UsuarioDespachador2\\otra mas",
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
                ExtensionEvento ext2 = new ExtensionEvento()
                {
                    Id = 2,
                    DescripcionDespachador = DateTime.Now.ToString(formato) + "\\UsuarioDespachador\\descripcion de evento\\" + DateTime.Now.ToString(formato) + "\\UsuarioDespachador2\\otra descripcion de evento\\" + DateTime.Now.ToString(formato) + "\\UsuarioDespachador2\\otra mas",
                    Evento = evento2,
                    Zona = zona2,
                    Estado = EstadoExtension.FaltaDespachar,
                    TimeStamp = DateTime.Now
                };
                ExtensionEvento ext3 = new ExtensionEvento()
                {
                    Id = 3,
                    DescripcionDespachador = DateTime.Now.ToString(formato) + "\\UsuarioDespachador\\descripcion de evento\\" + DateTime.Now.ToString(formato) + "\\UsuarioDespachador2\\otra descripcion de evento\\" + DateTime.Now.ToString(formato) + "\\UsuarioDespachador2\\otra mas",
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
                ext1.GeoUbicaciones.Add(geo3);
                ext1.GeoUbicaciones.Add(geo4);

                ext1.Recursos.Add(rec1);

                context.ExtensionesEvento.AddOrUpdate(x => x.Id, ext1);
                context.ExtensionesEvento.AddOrUpdate(x => x.Id, ext2);
                context.ExtensionesEvento.AddOrUpdate(x => x.Id, ext3);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
