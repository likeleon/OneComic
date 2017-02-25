using Core.Common.Contracts;
using OneComic.Business.Entities;
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
        private static readonly IReadOnlyList<Book> EmptyBooks = new List<Book>().AsReadOnly();

        private readonly IComicRepository _comicRepository;

        [ImportingConstructor]
        public BookRepository(IComicRepository comicRepository)
        {
            _comicRepository = comicRepository;
        }

        protected override Book AddEntity(OneComicContext context, Book entity)
        {
            return context.BookSet.Add(entity);
        }

        protected override IQueryable<Book> GetEntities(OneComicContext context)
        {
            return context.BookSet;
        }

        protected override Book GetEntity(OneComicContext context, int id)
        {
            return context.BookSet.FirstOrDefault(b => b.BookId == id);
        }

        protected override void AttachEntity(OneComicContext context, Book entity)
        {
            context.BookSet.Attach(entity);
        }

        public IReadOnlyList<Book> GetByComicId(int comicId)
        {
            using (var context = new OneComicContext())
                return GetBooksByComicIdQuery(context, comicId).ToList();
        }

        private IQueryable<Book> GetBooksByComicIdQuery(OneComicContext context, int comicId)
        {
            var comic = _comicRepository.Get(comicId);
            if (comic == null)
                return Enumerable.Empty<Book>().AsQueryable();

            return context.BookSet.Where(book => book.ComicId == comicId);
        }

        public DataPage<Book> GetByComicId(int comicId, string order, int page, int pageSize)
        {
            using (var context = new OneComicContext())
            {
                var query = GetBooksByComicIdQuery(context, comicId);
                return GetPage(query, order, page, pageSize);
            }
        }
    }
}
