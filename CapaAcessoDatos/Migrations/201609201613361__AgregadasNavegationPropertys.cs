namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _AgregadasNavegationPropertys : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Origen_Eventos", new[] { "Evento_Id" });
            DropColumn("dbo.Origen_Eventos", "Id");
            RenameColumn(table: "dbo.Origen_Eventos", name: "Evento_Id", newName: "Id");
            DropPrimaryKey("dbo.Origen_Eventos");
            AddColumn("dbo.Evento", "Evento_Id", c => c.Int());
            AddColumn("dbo.Sectores", "Zona_Id", c => c.Int());
            AlterColumn("dbo.Origen_Eventos", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Origen_Eventos", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Origen_Eventos", "Id");
            CreateIndex("dbo.Evento", "Evento_Id");
            CreateIndex("dbo.Sectores", "Zona_Id");
            CreateIndex("dbo.Origen_Eventos", "Id");
            AddForeignKey("dbo.Evento", "Evento_Id", "dbo.Evento", "Id");
            AddForeignKey("dbo.Sectores", "Zona_Id", "dbo.Zonas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sectores", "Zona_Id", "dbo.Zonas");
            DropForeignKey("dbo.Evento", "Evento_Id", "dbo.Evento");
            DropIndex("dbo.Origen_Eventos", new[] { "Id" });
            DropIndex("dbo.Sectores", new[] { "Zona_Id" });
            DropIndex("dbo.Evento", new[] { "Evento_Id" });
            DropPrimaryKey("dbo.Origen_Eventos");
            AlterColumn("dbo.Origen_Eventos", "Id", c => c.Int());
            AlterColumn("dbo.Origen_Eventos", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Sectores", "Zona_Id");
            DropColumn("dbo.Evento", "Evento_Id");
            AddPrimaryKey("dbo.Origen_Eventos", "Id");
            RenameColumn(table: "dbo.Origen_Eventos", name: "Id", newName: "Evento_Id");
            AddColumn("dbo.Origen_Eventos", "Id", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Origen_Eventos", "Evento_Id");
        }
    }
}
