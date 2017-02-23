using Core.Common.Contracts;
using OneComic.Business.Entities;
using System.Collections.Generic;

namespace OneComic.Data.Contracts
{
    public interface IBookRepository : IDataRepository<Book>
    {
        IReadOnlyList<Book> GetByComicId(int comicId);
    }
}
