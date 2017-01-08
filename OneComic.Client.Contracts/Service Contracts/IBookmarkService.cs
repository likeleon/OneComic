using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Client.Entities;
using OneComic.Common;
using System.ServiceModel;
using System.Threading.Tasks;

namespace OneComic.Client.Contracts
{
    [ServiceContract]
    public interface IBookmarkService : IServiceContract
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(PageNumberOutOfRangeException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Bookmark AddBookmark(string loginEmail, int bookId, int pageNumber);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        void RemoveBookmark(int bookmarkId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        AccountBookmarkData[] GetBookmarks(string loginEmail);

        [OperationContract]
        Task<Bookmark> AddBookmarkAsync(string loginEmail, int bookId, int pageNumber);

        [OperationContract]
        Task RemoveBookmarkAsync(int bookmarkId);

        [OperationContract]
        Task<AccountBookmarkData[]> GetBookmarksAsync(string loginEmail);
    }
}
