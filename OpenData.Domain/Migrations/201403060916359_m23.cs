namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m23 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authorities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        INN = c.Long(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Authorities");
        }
    }
}
