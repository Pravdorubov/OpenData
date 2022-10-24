namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OpenDataSets", "Periodicity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OpenDataSets", "Periodicity");
        }
    }
}
