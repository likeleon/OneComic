using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;
using System.Linq;

namespace OneComic.Data
{
    [Export(typeof(IAccountRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class AccountRepository : DataRepository<Account>, IAccountRepository
    {
        protected override Account AddEntity(OneComicContext context, Account entity)
        {
            return context.AccountSet.Add(entity);
        }

        protected override IQueryable<Account> GetEntities(OneComicContext context)
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
