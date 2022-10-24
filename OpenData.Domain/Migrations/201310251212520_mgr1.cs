namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OpenDataSets", "ODID", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OpenDataSets", "ODID", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
