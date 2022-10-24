namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenDataSets", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OpenDataSets", "CreateDate");
        }
    }
}
