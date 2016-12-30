using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace OneComic.Data
{
    [Export(typeof(IBookmarkRepository))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class BookmarkRepository : DataRepository<Bookmark>, IBookmarkRepository
    {
        protected override Bookmark AddEntity(OneComicContext context, Bookmark entity)
        {
            return context.BookmarkSet.Add(entity);
        }

        protected override IEnumerable<Bookmark> GetEntities(OneComicContext context)
        {
            return context.BookmarkSet;
        }

        protected override Bookmark GetEntity(OneComicContext context, int id)
        {
            return context.BookmarkSet.FirstOrDefault(b => b.BookId == id);
        }

        public IEnumerable<AccountBookmarkInfo> GetAccountBookmarkInfo(int accountId)
        {
            using (var context = new OneComicContext())
            {
                var query = from bookmark in context.BookmarkSet
                            join account in context.AccountSet on bookmark.AccountId equals account.AccountId
                            join book in context.BookSet on bookmark.BookId equals book.BookId
                            where bookmark.AccountId == accountId
                            select new AccountBookmarkInfo
                            {
                                Account = account,
                                Book = book,
                                Bookmark = bookmark
                            };
                return query.ToList();
            }
        }
    }
}
