namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfiles", "AuthorityINN", "dbo.cat_Authority");
            DropIndex("dbo.UserProfiles", new[] { "AuthorityINN" });
            AddColumn("dbo.UserProfiles", "cat_Authority_INN", c => c.Long());
            AddColumn("dbo.Users", "AuthorityINN", c => c.Long());
            AddColumn("dbo.Roles", "Description", c => c.String());
            AddForeignKey("dbo.UserProfiles", "cat_Authority_INN", "dbo.cat_Authority", "INN");
            AddForeignKey("dbo.Users", "AuthorityINN", "dbo.cat_Authority", "INN");
            CreateIndex("dbo.UserProfiles", "cat_Authority_INN");
            CreateIndex("dbo.Users", "AuthorityINN");
            DropColumn("dbo.UserProfiles", "AuthorityINN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "AuthorityINN", c => c.Long());
            DropIndex("dbo.Users", new[] { "AuthorityINN" });
            DropIndex("dbo.UserProfiles", new[] { "cat_Authority_INN" });
            DropForeignKey("dbo.Users", "AuthorityINN", "dbo.cat_Authority");
            DropForeignKey("dbo.UserProfiles", "cat_Authority_INN", "dbo.cat_Authority");
            DropColumn("dbo.Roles", "Description");
            DropColumn("dbo.Users", "AuthorityINN");
            DropColumn("dbo.UserProfiles", "cat_Authority_INN");
            CreateIndex("dbo.UserProfiles", "AuthorityINN");
            AddForeignKey("dbo.UserProfiles", "AuthorityINN", "dbo.cat_Authority", "INN");
        }
    }
}
