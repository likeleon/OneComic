using OneComic.Business.Entities;
using System.ComponentModel.Composition;

namespace OneComic.Data
{
    [Export(typeof(IAccountMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class AccountMapper : DataMapper<Account, DTO.Account>, IAccountMapper
    {
        public override DTO.Account ToDTO(Account account)
        {
            return new DTO.Account
            {
                AccountId = account.AccountId,
                LoginEmail = account.LoginEmail
            };
        }

        public override Account ToEntity(DTO.Account account)
        {
            return new Account
            {
                AccountId = account.AccountId,
                LoginEmail = account.LoginEmail
            };
        }
    }
}
