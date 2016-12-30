using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Business.Common;
using OneComic.Business.Contracts;
using OneComic.Business.Entities;
using OneComic.Common;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;

namespace OneComic.Business.Managers.Managers
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public sealed class BookmarkManager : Manager, IBookmarkService
    {
#pragma warning disable 0649
        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;

        [Import]
        private IBusinessEngineFactory _businessEngineFactory;
#pragma warning restore 0649

        [OperationBehavior(TransactionScopeRequired = true)]
        public Bookmark AddBookmark(string loginEmail, int bookId, int pageNumber)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var oneComicEngine = _businessEngineFactory.GetBusinessEngine<IOneComicEngine>();
                try
                {
                    return oneComicEngine.AddBookmark(loginEmail, bookId, pageNumber);
                }
                catch (OutOfRangePageNumberException ex)
                {
                    throw new FaultException<OutOfRangePageNumberException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void RemoveBookmark(int bookmarkId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                var bookmarkRepository = _dataRepositoryFactory.GetDataRepository<IBookmarkRepository>();
                var bookmark = bookmarkRepository.Get(bookmarkId);
                if (bookmark == null)
                {
                    var ex = new NotFoundException($"No bookmark found for ID '{bookmarkId}'.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                bookmarkRepository.Remove(bookmarkId);
            });
        }

        public AccountBookmarkData[] GetBookmarks(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                var account = accountRepository.GetByLoginEmail(loginEmail);
                if (account == null)
                {
                    var ex = new NotFoundException($"No account found for login email '{loginEmail}'.");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                var bookmarkRepository = _dataRepositoryFactory.GetDataRepository<IBookmarkRepository>();
                var bookmarkInfos = bookmarkRepository.GetAccountBookmarkInfo(account.AccountId);
                return bookmarkInfos.Select(b => new AccountBookmarkData
                {
                    BookmarkId = b.Bookmark.BookmarkId,
                    AccountLoginEmail = b.Account.LoginEmail,
                    BookTitle = b.Book.Title,
                    PageNumber = b.Bookmark.PageNumber
                }).ToArray();
            });
        }
    }
}
