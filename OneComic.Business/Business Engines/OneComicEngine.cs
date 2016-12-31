using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Business.Common;
using OneComic.Business.Entities;
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
            // TODO: Implement
            return true;
        }

        public Bookmark AddBookmark(Account account, int bookId, int pageNumber)
        {
            if (!IsPageNumberInRange(bookId, pageNumber))
                throw new NotFoundException($"Page number '{pageNumber} is out of range in book id '{bookId}'.");

            var bookmark = new Bookmark
            {
                AccountId = account.AccountId,
                BookId = bookId,
                PageNumber = pageNumber,
                DateCreated = DateTime.UtcNow,
            };

            var bookmarkRepository = _dataRepositoryFactory.GetDataRepository<IBookmarkRepository>();
            return bookmarkRepository.Add(bookmark);
        }
    }
}
