using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Client.Entities;
using OneComic.Common;
using System.ServiceModel;
using System.Threading.Tasks;

namespace OneComic.Client.Contracts
{
    [ServiceContract]
    public interface IAccountService : IServiceContract
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Account GetAccount(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateAccount(Account account);

        [OperationContract]
        Task<Account> GetAccountAsync(string loginEmail);

        [OperationContract]
        Task UpdateAccountAsync(Account account);
    }
}
