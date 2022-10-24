namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OpenDataSets", "cat_Authority_INN", "dbo.cat_Authority");
            DropIndex("dbo.OpenDataSets", new[] { "cat_Authority_INN" });
            DropColumn("dbo.OpenDataSets", "cat_Authority_INN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OpenDataSets", "cat_Authority_INN", c => c.Long());
            CreateIndex("dbo.OpenDataSets", "cat_Authority_INN");
            AddForeignKey("dbo.OpenDataSets", "cat_Authority_INN", "dbo.cat_Authority", "INN");
        }
    }
}
