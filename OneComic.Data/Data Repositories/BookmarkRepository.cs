﻿using OneComic.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace OneComic.Data
{
    [Export(typeof(IBookmarkRepository))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class BookmarkRepository : DataRepository<Bookmark, OneComicContext>, IBookmarkRepository
    {
        protected override Bookmark AddEntity(OneComicContext context, Bookmark entity)
        {
            return context.BookmarkSet.Add(entity);
        }

        protected override IQueryable<Bookmark> GetEntities(OneComicContext context)
        {
            return context.BookmarkSet;
        }

        protected override Bookmark GetEntity(OneComicContext context, int id)
        {
            return context.BookmarkSet.FirstOrDefault(b => b.BookId == id);
        }

        protected override void AttachEntity(OneComicContext context, Bookmark entity)
        {
            context.BookmarkSet.Attach(entity);
        }

        public IReadOnlyList<Bookmark> GetByAccountId(int accountId)
        {
            using (var context = new OneComicContext())
                return context.BookmarkSet.Where(bookmark => bookmark.AccountId == accountId).ToList();
        }
    }
}
