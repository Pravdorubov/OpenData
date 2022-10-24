namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Versions", "RowsCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Versions", "RowsCount");
        }
    }
}
