using OneComic.Business.Entities;

namespace OneComic.Data
{
    public interface IAccountRepository : IDataRepository<Account>
    {
        Account GetByLoginEmail(string loginEmail);
    }
}
