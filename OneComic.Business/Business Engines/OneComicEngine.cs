using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Business.Common;
using OneComic.Business.Entities;
using OneComic.Common;
using OneComic.Data.Contracts;
using System;
using System.ComponentModel.Composition;

namespace OneComic.Business
{
    [Export(typeof(IOneComicEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class OneComicEngine : IOneComicEngine
    {
        private readonly IDataRepositoryFactory _dataRepositoryFactory;

        [ImportingConstructor]
        public OneComicEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        public bool IsPageNumberInRange(int bookId, int pageNumber)
        {
            var bookRepository = _dataRepositoryFactory.GetDataRepository<IBookRepository>();
            var book = bookRepository.Get(bookId);
            if (book == null)
                throw new NotFoundException($"No book found for book id '{bookId}'.");

            return pageNumber < book.PageCount;
        }

        public Bookmark AddBookmark(Account account, int bookId, int pageNumber)
        {
            if (!IsPageNumberInRange(bookId, pageNumber))
                throw new PageNumberOutOfRangeException($"Page number '{pageNumber} is out of range in book id '{bookId}'.");

            var bookmark = new Bookmark
            {
                AccountId = account.AccountId,
                BookId = bookId,
                PageNumber = pageNumber,
                DateCreated = DateTime.UtcNow,
            };

            var bookmarkRepository = _dataRepositoryFactory.GetDataRepository<IBookmarkRepository>();
            return bookmarkRepository.Add(bookmark).Entity;
        }
    }
}
