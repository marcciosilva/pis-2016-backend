namespace Emsys.DataAccesLayer.Core
{
    using System.Data.Entity;
    using Model;
    
    public partial class EmsysContext : DbContext
    {
        public EmsysContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EmsysContext, Configuration>());
        }

        public void borrarBase()
        {
        }

        public DbSet<Evento> Evento { get; set; }

        public DbSet<ApplicationFile> ApplicationFiles { get; set; }

        public DbSet<Imagen> Imagenes { get; set; }

        public DbSet<Audio> Audios { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<GeoUbicacion> GeoUbicaciones { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<ExtensionEvento> ExtensionesEvento { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<OrigenEvento> OrigenEventos { get; set; }
        
        public DbSet<Recurso> Recursos { get; set; }

        public DbSet<Sector> Sectores { get; set; }

        public DbSet<UnidadEjecutora> UnidadesEjecutoras { get; set; }
        
        public DbSet<Zona> Zonas { get; set; }

        public DbSet<Rol> ApplicationRoles { get; set; }
        
        public DbSet<Permiso> Permisos { get; set; }

        public DbSet<GrupoRecurso> GruposRecursos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<AsignacionRecurso> AsignacionesRecursos { get; set; }

        public DbSet<LogNotification> LogNotification { get; set; }

        public DbSet<AsignacionRecursoDescripcion> AsignacionRecursoDescripcion { get; set; }
    }
}
