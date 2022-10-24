namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenDataSets", "AuthorityID", c => c.Int(nullable: false));
            AddColumn("dbo.UserProfiles", "AuthorityID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "AuthorityID");
            DropColumn("dbo.OpenDataSets", "AuthorityID");
        }
    }
}
