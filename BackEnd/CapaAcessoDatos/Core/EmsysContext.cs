namespace Emsys.DataAccesLayer.Core
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;

    //public class ApplicationUser : IdentityUser
    //{

    //}
    public partial class EmsysContext : IdentityDbContext
    {
        public EmsysContext() {
            //   Database.SetInitializer<EmsysContext>(new DropCreateDatabaseIfModelChanges<EmsysContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EmsysContext, Configuration>());
        }
        public DbSet<Evento> Evento { get; set; }

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
