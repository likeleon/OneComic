namespace OneComic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookCoverImageUri : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book", "CoverImageUri", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Book", "CoverImageUri");
        }
    }
}
