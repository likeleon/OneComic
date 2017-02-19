using Core.Common.Contracts;
using OneComic.Business.Entities;

namespace OneComic.Data.Contracts
{
    public interface IAccountRepository : IDataRepository<Account>
    {
        Account GetByLoginEmail(string loginEmail);
    }
}
