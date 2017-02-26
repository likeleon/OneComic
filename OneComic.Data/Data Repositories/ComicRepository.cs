using Core.Common.Contracts;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
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

        protected override IQueryable<Comic> GetEntities(OneComicContext context)
        {
            return context.ComicSet;
        }

        protected override Comic GetEntity(OneComicContext context, int id)
        {
            return context.ComicSet.FirstOrDefault(c => c.ComicId == id);
        }

        protected override void AttachEntity(OneComicContext context, Comic entity)
        {
            context.ComicSet.Attach(entity);
        }

        public IReadOnlyList<Comic> GetWithBooks()
        {
            return GetWithBooks(order: null);
        }

        public IReadOnlyList<Comic> GetWithBooks(string order)
        {
            using (var context = new OneComicContext())
                return GetWithBooksQuery(context).ToList();
        }

        private IQueryable<Comic> GetWithBooksQuery(OneComicContext context)
        {
            return GetEntities(context).Include(nameof(Comic.Books));
        }

        public DataPage<Comic> GetWithBooks(string order, int page, int pageSize)
        {
            using (var context = new OneComicContext())
            {
                var query = GetWithBooksQuery(context);
                return GetPage(query, order, page, pageSize);
            }
        }
    }
}
