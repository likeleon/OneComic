using Core.Common.Exceptions;
using OneComic.Business.Entities;
using OneComic.Common;
using System.ServiceModel;

namespace OneComic.Business.Contracts
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Account GetAccount(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateAccount(Account account);
    }
}
