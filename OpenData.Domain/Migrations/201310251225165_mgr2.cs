namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenDataSets", "CategorySt", c => c.String());
            DropColumn("dbo.OpenDataSets", "CategoryStr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OpenDataSets", "CategoryStr", c => c.String(nullable: false));
            DropColumn("dbo.OpenDataSets", "CategorySt");
        }
    }
}
