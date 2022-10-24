namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenDataSets", "HasGeo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OpenDataSets", "HasGeo");
        }
    }
}
