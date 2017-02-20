namespace OneComic.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        LoginEmail = c.String(),
                    })
                .PrimaryKey(t => t.AccountId);
            
            CreateTable(
                "dbo.Bookmark",
                c => new
                    {
                        BookmarkId = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        PageNumber = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookmarkId)
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        ComicId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Author = c.String(),
                        Translator = c.String(),
                        PageCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Comic", t => t.ComicId, cascadeDelete: true)
                .Index(t => t.ComicId);
            
            CreateTable(
                "dbo.Comic",
                c => new
                    {
                        ComicId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.ComicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookmark", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Bookmark", "BookId", "dbo.Book");
            DropForeignKey("dbo.Book", "ComicId", "dbo.Comic");
            DropIndex("dbo.Book", new[] { "ComicId" });
            DropIndex("dbo.Bookmark", new[] { "BookId" });
            DropIndex("dbo.Bookmark", new[] { "AccountId" });
            DropTable("dbo.Comic");
            DropTable("dbo.Book");
            DropTable("dbo.Bookmark");
            DropTable("dbo.Account");
        }
    }
}
