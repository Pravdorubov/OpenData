namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "AuthorityINN", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "AuthorityINN", c => c.Int(nullable: false));
        }
    }
}
