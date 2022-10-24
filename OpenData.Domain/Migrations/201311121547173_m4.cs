namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m4 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OpenDataSets", "AuthorityINN");
            AddForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority", "INN", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OpenDataSets", "AuthorityINN", "dbo.cat_Authority");
            DropIndex("dbo.OpenDataSets", new[] { "AuthorityINN" });
        }
    }
}
