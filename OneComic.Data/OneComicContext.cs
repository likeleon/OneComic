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
            : base("name=OneComic")
        {
            Database.SetInitializer<OneComicContext>(null);
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

            modelBuilder.Entity<Account>().HasKey(a => a.AccountId).Ignore(a => a.EntityId);
            modelBuilder.Entity<Comic>().HasKey(c => c.ComicId).Ignore(c => c.EntityId);
            modelBuilder.Entity<Book>().HasKey(b => b.BookId).Ignore(b => b.EntityId);
            modelBuilder.Entity<Bookmark>().HasKey(b => b.BookmarkId).Ignore(b => b.EntityId);
        }
    }
}
