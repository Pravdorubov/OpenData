namespace OpenData.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.cat_Category", "ImageData", c => c.Binary());
            AddColumn("dbo.cat_Category", "ImageMimeType", c => c.String());
            AlterColumn("dbo.cat_Category", "Description", c => c.String());
            DropColumn("dbo.OpenDataSets", "ImageData");
            DropColumn("dbo.OpenDataSets", "ImageMimeType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OpenDataSets", "ImageMimeType", c => c.String());
            AddColumn("dbo.OpenDataSets", "ImageData", c => c.Binary());
            AlterColumn("dbo.cat_Category", "Description", c => c.String(nullable: false));
            DropColumn("dbo.cat_Category", "ImageMimeType");
            DropColumn("dbo.cat_Category", "ImageData");
        }
    }
}
