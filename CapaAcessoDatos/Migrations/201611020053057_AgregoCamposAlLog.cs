namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregoCamposAlLog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogNotification", "Topic", c => c.String());
            AddColumn("dbo.LogNotification", "CodigoNotificacion", c => c.String());
            AddColumn("dbo.LogNotification", "PKEventoAfectado", c => c.String());
            AddColumn("dbo.LogNotification", "responseFireBase", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LogNotification", "responseFireBase");
            DropColumn("dbo.LogNotification", "PKEventoAfectado");
            DropColumn("dbo.LogNotification", "CodigoNotificacion");
            DropColumn("dbo.LogNotification", "Topic");
        }
    }
}
