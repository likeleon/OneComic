using System.Collections.Generic;

namespace Core.Common.Contracts
{
    public interface IDataRepository
    {
    }

    public class DataPage<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Entities { get; set; }
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
