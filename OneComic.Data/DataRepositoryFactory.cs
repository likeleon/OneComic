using Core.Common.Contracts;
using Core.Common.Core;
using System.ComponentModel.Composition;

namespace OneComic.Data
{
    [Export(typeof(IDataRepositoryFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class DataRepositoryFactory : IDataRepositoryFactory
    {
        public T GetDataRepository<T>() where T : IDataRepository
        {
            return Global.Container.GetExportedValue<T>();
        }
    }
}
