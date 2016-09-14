namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _AgregoTablaZona : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Zona",
                c => new
                    {
                        NombreZona = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.NombreZona);
            
            CreateTable(
                "dbo.ZonaEventoes",
                c => new
                    {
                        Zona_NombreZona = c.String(nullable: false, maxLength: 128),
                        Evento_NombreGenerador = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Zona_NombreZona, t.Evento_NombreGenerador })
                .ForeignKey("dbo.Zona", t => t.Zona_NombreZona, cascadeDelete: true)
                .ForeignKey("dbo.Evento", t => t.Evento_NombreGenerador, cascadeDelete: true)
                .Index(t => t.Zona_NombreZona)
                .Index(t => t.Evento_NombreGenerador);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZonaEventoes", "Evento_NombreGenerador", "dbo.Evento");
            DropForeignKey("dbo.ZonaEventoes", "Zona_NombreZona", "dbo.Zona");
            DropIndex("dbo.ZonaEventoes", new[] { "Evento_NombreGenerador" });
            DropIndex("dbo.ZonaEventoes", new[] { "Zona_NombreZona" });
            DropTable("dbo.ZonaEventoes");
            DropTable("dbo.Zona");
        }
    }
}
