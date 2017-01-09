using Core.Common.Contracts;
using Core.Common.Core;
using System.ComponentModel.Composition;

namespace OneComic.Client.Proxies
{
    [Export(typeof(IServiceFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class ServiceFactory : IServiceFactory
    {
        public T CreateClient<T>() where T : IServiceContract
        {
            return Global.Container.GetExportedValue<T>();
        }
    }
}
