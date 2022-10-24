namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Versions", "VerNum", c => c.String());
            AlterColumn("dbo.Structures", "VerNum", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Structures", "VerNum", c => c.Int(nullable: false));
            AlterColumn("dbo.Versions", "VerNum", c => c.Int(nullable: false));
        }
    }
}
