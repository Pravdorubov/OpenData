namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "AuthorityINN", "dbo.cat_Authority");
            DropForeignKey("dbo.OpenDataSets", "User_ID", "dbo.Users");
            DropIndex("dbo.Users", new[] { "AuthorityINN" });
            DropIndex("dbo.OpenDataSets", new[] { "User_ID" });
            DropColumn("dbo.OpenDataSets", "User_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OpenDataSets", "User_ID", c => c.Int());
            CreateIndex("dbo.OpenDataSets", "User_ID");
            CreateIndex("dbo.Users", "AuthorityINN");
            AddForeignKey("dbo.OpenDataSets", "User_ID", "dbo.Users", "ID");
            AddForeignKey("dbo.Users", "AuthorityINN", "dbo.cat_Authority", "INN");
        }
    }
}
