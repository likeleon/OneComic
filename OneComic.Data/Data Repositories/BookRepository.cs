﻿using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace OneComic.Data
{
    [Export(typeof(IBookRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class BookRepository : DataRepository<Book>, IBookRepository
    {
        protected override Book AddEntity(OneComicContext context, Book entity)
        {
            return context.BookSet.Add(entity);
        }

        protected override IEnumerable<Book> GetEntities(OneComicContext context)
        {
            return context.BookSet;
        }

        protected override Book GetEntity(OneComicContext context, int id)
        {
            return context.BookSet.FirstOrDefault(b => b.BookId == id);
        }
    }
}