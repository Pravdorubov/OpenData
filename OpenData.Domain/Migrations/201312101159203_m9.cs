namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        RoleID = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        UserProfile_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserProfile_ID)
                .Index(t => t.RoleID)
                .Index(t => t.UserProfile_ID);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Password = c.String(nullable: false),
                        Email = c.String(),
                        Phone = c.String(),
                        FNS = c.String(nullable: false),
                        Duty = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserProfile_ID", "dbo.UserProfiles");
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "UserProfile_ID" });
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
        }
    }
}
