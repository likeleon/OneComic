using OneComic.Business.Entities;
using System.Collections.Generic;

namespace OneComic.Data.Contracts
{
    public interface IBookmarkRepository
    {
        IEnumerable<AccountBookmarkInfo> GetAccountBookmarkInfo(int accountId);
    }
}
