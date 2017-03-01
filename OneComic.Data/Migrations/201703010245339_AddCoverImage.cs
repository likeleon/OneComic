namespace OneComic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoverImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comic", "CoverImageUri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comic", "CoverImageUri");
        }
    }
}
