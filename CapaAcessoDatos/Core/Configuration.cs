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
            // Categorias de prueba
            context.Categorias.AddOrUpdate(new Categoria { Codigo = "A123", Clave = "estoesunaprueba", Prioridad = NombrePrioridad.Alta, Activo = false });
            context.Categorias.AddOrUpdate(new Categoria { Codigo = "B321", Clave = "estoesunaprueba2", Prioridad = NombrePrioridad.Media, Activo = false });
            context.Categorias.AddOrUpdate(new Categoria { Codigo = "C213", Clave = "estoesunaprueba3", Prioridad = NombrePrioridad.Baja, Activo = false });

            // Departamentos de prueba
            context.Departamentos.AddOrUpdate(new Departamento { Nombre = "dep1" });
            context.Departamentos.AddOrUpdate(new Departamento { Nombre = "dep2" });
            context.Departamentos.AddOrUpdate(new Departamento { Nombre = "dep3" });

        }
    }
}
