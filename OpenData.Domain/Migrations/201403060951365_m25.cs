namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m25 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority");
            DropForeignKey("dbo.UserProfiles", "AuthorityINN", "dbo.cat_Authority");
            DropIndex("dbo.OpenDataSets", new[] { "AuthorityINN" });
            DropIndex("dbo.UserProfiles", new[] { "AuthorityINN" });
            CreateIndex("dbo.OpenDataSets", "AuthorityID");
            CreateIndex("dbo.UserProfiles", "AuthorityID");
            AddForeignKey("dbo.OpenDataSets", "AuthorityID", "dbo.Authorities", "ID", cascadeDelete: true);
            AddForeignKey("dbo.UserProfiles", "AuthorityID", "dbo.Authorities", "ID", cascadeDelete: true);
            DropTable("dbo.cat_Authority");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.cat_Authority",
                c => new
                    {
                        INN = c.Long(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.INN);
            
            DropForeignKey("dbo.UserProfiles", "AuthorityID", "dbo.Authorities");
            DropForeignKey("dbo.OpenDataSets", "AuthorityID", "dbo.Authorities");
            DropIndex("dbo.UserProfiles", new[] { "AuthorityID" });
            DropIndex("dbo.OpenDataSets", new[] { "AuthorityID" });
            CreateIndex("dbo.UserProfiles", "AuthorityINN");
            CreateIndex("dbo.OpenDataSets", "AuthorityINN");
            AddForeignKey("dbo.UserProfiles", "AuthorityINN", "dbo.cat_Authority", "INN", cascadeDelete: true);
            AddForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority", "INN", cascadeDelete: true);
        }
    }
}
