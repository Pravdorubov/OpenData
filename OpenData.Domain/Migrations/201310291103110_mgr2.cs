namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.UserProfiles", name: "Authority_INN", newName: "AuthorityINN");
            AlterColumn("dbo.UserProfiles", "AuthorityINN", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "AuthorityINN", c => c.Int());
            RenameColumn(table: "dbo.UserProfiles", name: "AuthorityINN", newName: "Authority_INN");
        }
    }
}
