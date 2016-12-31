using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Business.Contracts;
using OneComic.Business.Entities;
using OneComic.Common;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;
using System.Security.Permissions;
using System.ServiceModel;

namespace OneComic.Business.Managers.Managers
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        ReleaseServiceInstanceOnTransactionComplete = false)]
    public sealed class AccountManager : Manager, IAccountService
    {
#pragma warning disable 0649
        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;
#pragma warning restore 0649

        protected override Account GetAuthorizationValidationAccount(string loginName)
        {
            return AccountAuthorization.GetAccount(_dataRepositoryFactory, loginName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.OneComicAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.OneComicUser)]
        public Account GetAccount(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                var account = accountRepository.GetByLoginEmail(loginEmail);
                if (account == null)
                {
                    var ex = new NotFoundException($"Account with login {loginEmail} is not in database");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);

                return account;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.OneComicAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.OneComicUser)]
        public void UpdateAccount(Account account)
        {
            ExecuteFaultHandledOperation(() =>
            {
                ValidateAuthorization(account);

                var accountRepository = _dataRepositoryFactory.GetDataRepository<IAccountRepository>();
                accountRepository.Update(account);
            });
        }
    }
}
