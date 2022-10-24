namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Link = c.String(),
                        Controller = c.String(),
                        Action = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.UserProfiles", "AuthorityINN", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "AuthorityINN");
            DropTable("dbo.Menus");
        }
    }
}
