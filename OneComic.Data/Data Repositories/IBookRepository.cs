using OneComic.Data.Entities;
using System.Collections.Generic;

namespace OneComic.Data
{
    public interface IBookRepository : IDataRepository<Book>
    {
        IReadOnlyList<Book> GetByComicId(int comicId);
        DataPage<Book> GetByComicId(int comicId, string order, int page, int pageSize);
    }
}
