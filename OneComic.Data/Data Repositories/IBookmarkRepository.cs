using OneComic.Data.Entities;
using System.Collections.Generic;

namespace OneComic.Data
{
    public interface IBookmarkRepository : IDataRepository<Bookmark>
    {
        IReadOnlyList<Bookmark> GetByAccountId(int accountId);
    }
}
