using Core.Common.Contracts;
using Core.Common.Data;

namespace OneComic.Data
{
    public abstract class DataRepository<T> : DataRepository<T, OneComicContext>
        where T : class, IIdentifiableEntity, new()
    {
    }
}
