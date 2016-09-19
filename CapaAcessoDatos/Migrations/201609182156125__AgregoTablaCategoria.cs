namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _AgregoTablaCategoria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categoria",
                c => new
                {
                    Codigo = c.String(nullable: false, maxLength: 128),
                    Clave = c.String(),
                    Prioridad = c.String(),
                    Activo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Codigo);

            AddColumn("dbo.Evento", "Categoria_Codigo", c => c.String(maxLength: 128));
            CreateIndex("dbo.Evento", "Categoria_Codigo");
            AddForeignKey("dbo.Evento", "Categoria_Codigo", "dbo.Categoria", "Codigo");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Evento", "Categoria_Codigo", "dbo.Categoria");
            DropIndex("dbo.Evento", new[] { "Categoria_Codigo" });
            DropColumn("dbo.Evento", "Categoria_Codigo");
            DropTable("dbo.Categoria");
        }
    }
}
