namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SNP = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.OpenDataSets", "OwnerID", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OpenDataSets", "OwnerID");
            DropTable("dbo.Owners");
        }
    }
}
