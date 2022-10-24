namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority");
            DropIndex("dbo.OpenDataSets", new[] { "AuthorityINN" });
            RenameColumn(table: "dbo.OpenDataSets", name: "AuthorityINN", newName: "cat_Authority_INN");
            AddForeignKey("dbo.OpenDataSets", "cat_Authority_INN", "dbo.cat_Authority", "INN");
            CreateIndex("dbo.OpenDataSets", "cat_Authority_INN");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OpenDataSets", new[] { "cat_Authority_INN" });
            DropForeignKey("dbo.OpenDataSets", "cat_Authority_INN", "dbo.cat_Authority");
            RenameColumn(table: "dbo.OpenDataSets", name: "cat_Authority_INN", newName: "AuthorityINN");
            CreateIndex("dbo.OpenDataSets", "AuthorityINN");
            AddForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority", "INN", cascadeDelete: true);
        }
    }
}
