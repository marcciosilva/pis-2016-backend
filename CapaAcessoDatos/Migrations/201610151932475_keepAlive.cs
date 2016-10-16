namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class keepAlive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "UltimoSignal", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "UltimoSignal");
        }
    }
}
