using Core.Common.Contracts;
using OneComic.Business.Entities;
using System.Linq;

namespace OneComic.Data.Contracts
{
    public interface IBookmarkRepository : IDataRepository<Bookmark>
    {
        IQueryable<Bookmark> GetByAccountId(int accountId);
    }
}
