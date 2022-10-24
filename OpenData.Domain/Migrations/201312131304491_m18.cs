namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m18 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.UserProfiles", "AuthorityINN");
            AddForeignKey("dbo.UserProfiles", "AuthorityINN", "dbo.cat_Authority", "INN", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfiles", "AuthorityINN", "dbo.cat_Authority");
            DropIndex("dbo.UserProfiles", new[] { "AuthorityINN" });
        }
    }
}
