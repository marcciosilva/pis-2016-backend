namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistrationTokenEnUsuarios : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "RegistrationTokenFirebase", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "RegistrationTokenFirebase");
        }
    }
}
