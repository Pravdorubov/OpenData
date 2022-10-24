namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority");
            DropIndex("dbo.OpenDataSets", new[] { "AuthorityINN" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.OpenDataSets", "AuthorityINN");
            AddForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority", "INN", cascadeDelete: true);
        }
    }
}
