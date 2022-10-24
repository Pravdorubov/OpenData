namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Structures", "IsCurrent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Structures", "IsCurrent", c => c.Boolean(nullable: false));
        }
    }
}
