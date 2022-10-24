namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m8 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OpenDataSets", "OwnerID");
            AddForeignKey("dbo.OpenDataSets", "OwnerID", "dbo.Owners", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OpenDataSets", "OwnerID", "dbo.Owners");
            DropIndex("dbo.OpenDataSets", new[] { "OwnerID" });
        }
    }
}
