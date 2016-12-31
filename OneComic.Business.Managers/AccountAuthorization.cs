using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.ServiceModel;

namespace OneComic.Business.Managers
{
    internal static class AccountAuthorization
    {
        public static Account GetAccount(IDataRepositoryFactory dataRepositoryFactory, string loginName)
        {
            var accountRepository = dataRepositoryFactory.GetDataRepository<IAccountRepository>();
            var authAccount = accountRepository.GetByLoginEmail(loginName);
            if (authAccount == null)
            {
                var ex = new NotFoundException($"Cannot find account for login email {loginName} to use for security trimming.");
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }

            return authAccount;
        }
    }
}
