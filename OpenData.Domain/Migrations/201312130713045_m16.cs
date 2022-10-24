namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OpenDataSets", "OwnerID", "dbo.Owners");
            DropIndex("dbo.OpenDataSets", new[] { "OwnerID" });
            CreateIndex("dbo.OpenDataSets", "UserID");
            AddForeignKey("dbo.OpenDataSets", "UserID", "dbo.Users", "ID", cascadeDelete: true);
            DropColumn("dbo.OpenDataSets", "OwnerID");
            DropTable("dbo.Owners");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SNP = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.OpenDataSets", "OwnerID", c => c.Int(nullable: false));
            DropForeignKey("dbo.OpenDataSets", "UserID", "dbo.Users");
            DropIndex("dbo.OpenDataSets", new[] { "UserID" });
            CreateIndex("dbo.OpenDataSets", "OwnerID");
            AddForeignKey("dbo.OpenDataSets", "OwnerID", "dbo.Owners", "ID", cascadeDelete: true);
        }
    }
}
