using Core.Common.Exceptions;
using OneComic.Business.Entities;
using OneComic.Common;
using System.ServiceModel;

namespace OneComic.Business.Contracts
{
    [ServiceContract]
    public interface IBookmarkService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(OutOfRangePageNumberException))]
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
    }
}
