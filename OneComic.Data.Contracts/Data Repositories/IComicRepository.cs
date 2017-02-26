using Core.Common.Contracts;
using OneComic.Business.Entities;
using System.Collections.Generic;

namespace OneComic.Data.Contracts
{
    public interface IComicRepository : IDataRepository<Comic>
    {
        IReadOnlyList<Comic> GetWithBooks();
        IReadOnlyList<Comic> GetWithBooks(string order);
        DataPage<Comic> GetWithBooks(string order, int page, int pageSize);
    }
}
