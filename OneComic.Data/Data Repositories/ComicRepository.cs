﻿using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace OneComic.Data
{
    [Export(typeof(IComicRepository))]
    public sealed class ComicRepository : DataRepository<Comic>, IComicRepository
    {
        protected override Comic AddEntity(OneComicContext context, Comic entity)
        {
            return context.ComicSet.Add(entity);
        }

        protected override IEnumerable<Comic> GetEntities(OneComicContext context)
        {
            return context.ComicSet;
        }

        protected override Comic GetEntity(OneComicContext context, int id)
        {
            return context.ComicSet.FirstOrDefault(c => c.ComicId == id);
        }
    }
}