namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity;
    using Model;
    
    public partial class EmsysContext : DbContext
    {
        public EmsysContext() : base("name=DefaultConnection")
        {
            //   Database.SetInitializer<EmsysContext>(new DropCreateDatabaseIfModelChanges<EmsysContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EmsysContext, Configuration>());
        }
        public DbSet<Evento> Evento { get; set; }

        public DbSet<ApplicationFile> ApplicationFiles { get; set; }

        public DbSet<Imagen> Imagenes { get; set; }

        public DbSet<Audio> Audios { get; set; }

        public DbSet<Video> Videos { get; set; }

        public DbSet<GeoUbicacion> GeoUbicaciones { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<Extension_Evento> Extensiones_Evento { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Origen_Evento> Origen_Eventos { get; set; }
        
        public DbSet<Recurso> Recursos { get; set; }

        public DbSet<Sector> Sectores { get; set; }

        public DbSet<Unidad_Ejecutora> Unidades_Ejecutoras { get; set; }
        
        public DbSet<Zona> Zonas { get; set; }

        public DbSet<Rol> ApplicationRoles { get; set; }
        
        public DbSet<Permiso> Permisos { get; set; }

        public DbSet<Grupo_Recurso> Grupos_Recursos { get; set; }

        public DbSet<Usuario> Users { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<IdentityUser>().ToTable("Users").
        //        Property(u => u.PasswordHash).HasColumnName("Password");
        //    modelBuilder.Entity<ApplicationUser>().ToTable("Users").
        //        Property(u => u.PasswordHash).HasColumnName("Password");
        //}
    }


}
