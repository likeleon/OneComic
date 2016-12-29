using Core.Common.Contracts;
using OneComic.Business.Entities;

namespace OneComic.Data.Contracts.Repository_Interfaces
{
    public interface IAccountRepository : IDataRepository<Account>
    {
        Account GetByLoginEmail(string loginEmail);
    }
}
