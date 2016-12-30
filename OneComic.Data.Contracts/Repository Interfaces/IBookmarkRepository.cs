using Core.Common.Contracts;
using OneComic.Business.Entities;
using System.Collections.Generic;

namespace OneComic.Data.Contracts
{
    public interface IBookmarkRepository : IDataRepository<Bookmark>
    {
        IEnumerable<AccountBookmarkInfo> GetAccountBookmarkInfo(int accountId);
    }
}
