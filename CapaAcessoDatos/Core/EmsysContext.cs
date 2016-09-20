namespace Emsys.DataAccesLayer.Core
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;

    //public class ApplicationUser : IdentityUser
    //{

    //}
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public partial class EmsysContext : IdentityDbContext
    {
        public EmsysContext() {
            //   Database.SetInitializer<EmsysContext>(new DropCreateDatabaseIfModelChanges<EmsysContext>());
            // Database.SetInitializer(new MigrateDatabaseToLatestVersion<EmsysContext, Configuration>());
            Database.SetInitializer(new MySqlInitializer());
        }
        public DbSet<Evento> Evento { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<Extension_Evento> Extension_Eventos { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Origen_Evento> Origen_Eventos { get; set; }

        public DbSet<Permiso> Permisos { get; set; }

        public DbSet<Recurso> Recursos { get; set; }

        public DbSet<Sector> Sectores { get; set; }

        public DbSet<Usuario_UE> Usuarios_UE { get; set; }

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
