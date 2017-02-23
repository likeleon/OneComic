using System.Collections.Generic;

namespace Core.Common.Contracts
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

        IReadOnlyList<T> Get(string order = null);
        T Get(int id);
    }
}
