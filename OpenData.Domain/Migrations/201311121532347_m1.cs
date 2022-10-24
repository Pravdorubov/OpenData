namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OpenDataSets", "UserID", "dbo.Users");
            DropIndex("dbo.OpenDataSets", new[] { "UserID" });
            RenameColumn(table: "dbo.OpenDataSets", name: "UserID", newName: "User_ID");
            AddColumn("dbo.OpenDataSets", "AuthorityINN", c => c.Long(nullable: true));
            AlterColumn("dbo.OpenDataSets", "User_ID", c => c.Int());
            //CreateIndex("dbo.OpenDataSets", "AuthorityINN");
            //CreateIndex("dbo.OpenDataSets", "User_ID");
            //AddForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority", "INN", cascadeDelete: true);
            //AddForeignKey("dbo.OpenDataSets", "User_ID", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OpenDataSets", "User_ID", "dbo.Users");
            DropForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority");
            DropIndex("dbo.OpenDataSets", new[] { "User_ID" });
            DropIndex("dbo.OpenDataSets", new[] { "AuthorityINN" });
            AlterColumn("dbo.OpenDataSets", "User_ID", c => c.Int(nullable: false));
            DropColumn("dbo.OpenDataSets", "AuthorityINN");
            RenameColumn(table: "dbo.OpenDataSets", name: "User_ID", newName: "UserID");
            CreateIndex("dbo.OpenDataSets", "UserID");
            AddForeignKey("dbo.OpenDataSets", "UserID", "dbo.Users", "ID", cascadeDelete: true);
        }
    }
}
