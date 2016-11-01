namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregoLogViejoLogNotificaciones : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LogNotification", "LogNotificationPrevio_Id", c => c.Int());
            CreateIndex("dbo.LogNotification", "LogNotificationPrevio_Id");
            AddForeignKey("dbo.LogNotification", "LogNotificationPrevio_Id", "dbo.LogNotification", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogNotification", "LogNotificationPrevio_Id", "dbo.LogNotification");
            DropIndex("dbo.LogNotification", new[] { "LogNotificationPrevio_Id" });
            DropColumn("dbo.LogNotification", "LogNotificationPrevio_Id");
        }
    }
}
