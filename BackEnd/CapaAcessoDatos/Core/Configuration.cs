namespace Emsys.DataAccesLayer.Core
{
    using Model;
    using System;
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
            context.Evento.AddOrUpdate(new Evento { NombreGenerador = "event1", Descripcion = "blablalblba", Direccion = "direccion", FechaCreacion = new DateTime(1980, 10, 10) });
            context.Evento.AddOrUpdate(new Evento { NombreGenerador = "event2", Descripcion = "blablaasdasdlblba", Direccion = "direccion", FechaCreacion = new DateTime(1980, 11, 12) });            

        }
    }
}
