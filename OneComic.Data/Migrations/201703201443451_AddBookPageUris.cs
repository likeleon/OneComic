namespace OneComic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookPageUris : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book", "PageUris", c => c.String());
            DropColumn("dbo.Book", "PageCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Book", "PageCount", c => c.Int(nullable: false));
            DropColumn("dbo.Book", "PageUris");
        }
    }
}
