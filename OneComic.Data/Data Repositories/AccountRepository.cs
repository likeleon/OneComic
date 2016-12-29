using OneComic.Business.Entities;
using OneComic.Data.Contracts.Repository_Interfaces;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace OneComic.Data.Data_Repositories
{
    [Export(typeof(IAccountRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountRepository : DataRepository<Account>, IAccountRepository
    {
        protected override Account AddEntity(OneComicContext context, Account entity)
        {
            return context.AccountSet.Add(entity);
        }

        protected override IEnumerable<Account> GetEntities(OneComicContext context)
        {
            return context.AccountSet;
        }

        protected override Account GetEntity(OneComicContext context, int id)
        {
            return context.AccountSet.FirstOrDefault(e => e.AccountId == id);
        }

        public Account GetByLoginEmail(string loginEmail)
        {
            using (var context = new OneComicContext())
                return context.AccountSet.FirstOrDefault(a => a.LoginEmail == loginEmail);
        }
    }
}
