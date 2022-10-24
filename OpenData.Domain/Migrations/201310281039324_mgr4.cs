namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mgr4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Versions",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ODID = c.String(maxLength: 128),
                        StructureID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Reason = c.Int(nullable: false),
                        VerNum = c.Int(nullable: false),
                        IsCurrent = c.Boolean(nullable: false),
                        File = c.Binary(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OpenDataSets", t => t.ODID)
                .ForeignKey("dbo.Structures", t => t.StructureID, cascadeDelete: true)
                .Index(t => t.ODID)
                .Index(t => t.StructureID);
            
            CreateTable(
                "dbo.Structures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        VerNum = c.Int(nullable: false),
                        isCurrent = c.Boolean(nullable: false),
                        File = c.Binary(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Versions", new[] { "StructureID" });
            DropIndex("dbo.Versions", new[] { "ODID" });
            DropForeignKey("dbo.Versions", "StructureID", "dbo.Structures");
            DropForeignKey("dbo.Versions", "ODID", "dbo.OpenDataSets");
            DropTable("dbo.Structures");
            DropTable("dbo.Versions");
        }
    }
}
