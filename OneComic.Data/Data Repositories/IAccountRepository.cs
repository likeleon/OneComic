using OneComic.Data.Entities;

namespace OneComic.Data
{
    public interface IAccountRepository : IDataRepository<Account>
    {
        Account GetByLoginEmail(string loginEmail);
    }
}
