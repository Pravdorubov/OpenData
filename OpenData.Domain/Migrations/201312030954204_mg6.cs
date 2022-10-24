namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Users", "UserProfile_ID", "dbo.UserProfiles");
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Users", new[] { "UserProfile_ID" });
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.UserProfiles");
        }
        
        public override void Down()
        {
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        RoleID = c.Int(nullable: false),
                        AuthorityINN = c.Long(),
                        UserProfile_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Users", "UserProfile_ID");
            CreateIndex("dbo.Users", "RoleID");
            AddForeignKey("dbo.Users", "UserProfile_ID", "dbo.UserProfiles", "ID");
            AddForeignKey("dbo.Users", "RoleID", "dbo.Roles", "ID", cascadeDelete: true);
        }
    }
}
