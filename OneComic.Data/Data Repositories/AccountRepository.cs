using OneComic.Business.Entities;
using System.ComponentModel.Composition;
using System.Linq;

namespace OneComic.Data
{
    [Export(typeof(IAccountRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class AccountRepository : DataRepository<Account, OneComicContext>, IAccountRepository
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

        protected override void AttachEntity(OneComicContext context, Account entity)
        {
            context.AccountSet.Attach(entity);
        }

        public Account GetByLoginEmail(string loginEmail)
        {
            using (var context = new OneComicContext())
                return context.AccountSet.FirstOrDefault(a => a.LoginEmail == loginEmail);
        }
    }
}
