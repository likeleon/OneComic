using Core.Common.Contracts;
using OneComic.Business.Entities;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Runtime.Serialization;

namespace OneComic.Data
{
    public class OneComicContext : DbContext
    {
        public OneComicContext()
            : base("name=OneComicContext")
        {
            Database.SetInitializer<OneComicContext>(null);

            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Account> AccountSet { get; set; }
        public DbSet<Comic> ComicSet { get; set; }
        public DbSet<Book> BookSet { get; set; }
        public DbSet<Bookmark> BookmarkSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Ignore<PropertyChangedEventHandler>();
            modelBuilder.Ignore<ExtensionDataObject>();
            modelBuilder.Ignore<IIdentifiableEntity>();

            modelBuilder.Entity<Account>()
                .HasKey(account => account.AccountId)
                .Ignore(account => account.EntityId);

            modelBuilder.Entity<Account>()
                .HasMany(account => account.Bookmarks)
                .WithRequired(bookmark => bookmark.Account)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Comic>()
                .HasKey(comic => comic.ComicId)
                .Ignore(comic => comic.EntityId);

            modelBuilder.Entity<Comic>()
                .HasMany(comic => comic.Books)
                .WithRequired(book => book.Comic)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Book>()
                .HasKey(book => book.BookId)
                .Ignore(book => book.EntityId);

            modelBuilder.Entity<Bookmark>()
                .HasKey(bookmark => bookmark.BookmarkId)
                .Ignore(bookmark => bookmark.EntityId);
        }
    }
}
