using Core.Common.Contracts;
using Core.Common.Exceptions;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;

namespace OneComic.Business.Managers
{
    internal static class AccountAuthorization
    {
        public static Account Authorize(IDataRepositoryFactory dataRepositoryFactory, string loginName)
        {
            var accountRepository = dataRepositoryFactory.GetDataRepository<IAccountRepository>();
            var authAccount = accountRepository.GetByLoginEmail(loginName);
            if (authAccount == null)
                throw new NotFoundException($"Cannot find account for login email {loginName} to use for security trimming.");

            return authAccount;
        }
    }
}
