using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;

namespace OneComic.Data.Mappers
{
    [Export(typeof(IAccountMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class AccountMapper : IAccountMapper
    {
        public DTO.Account ToDTO(Account account)
        {
            return new DTO.Account
            {
                AccountId = account.AccountId,
                LoginEmail = account.LoginEmail
            };
        }

        public Account ToEntity(DTO.Account account)
        {
            return new Account
            {
                AccountId = account.AccountId,
                LoginEmail = account.LoginEmail
            };
        }
    }
}
