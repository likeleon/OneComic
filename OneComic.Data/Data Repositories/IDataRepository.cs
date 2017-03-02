using OneComic.Core;
using System.Collections.Generic;

namespace OneComic.Data
{
    public interface IDataRepository
    {
    }

    public interface IDataRepository<T> : IDataRepository
        where T : class, IIdentifiableEntity, new()
    {
        RepositoryActionResult<T> Add(T entity);

        RepositoryActionResult<T> Remove(T entity);
        RepositoryActionResult<T> Remove(int id);

        RepositoryActionResult<T> Update(T entity);

        IReadOnlyList<T> Get();
        IReadOnlyList<T> Get(string order);
        DataPage<T> Get(string order, int page, int pageSize);

        T Get(int id);
    }
}
