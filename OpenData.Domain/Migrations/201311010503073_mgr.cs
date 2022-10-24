namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfiles", "cat_Authority_INN", "dbo.cat_Authority");
            DropIndex("dbo.UserProfiles", new[] { "cat_Authority_INN" });
            DropColumn("dbo.UserProfiles", "cat_Authority_INN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "cat_Authority_INN", c => c.Long());
            CreateIndex("dbo.UserProfiles", "cat_Authority_INN");
            AddForeignKey("dbo.UserProfiles", "cat_Authority_INN", "dbo.cat_Authority", "INN");
        }
    }
}
