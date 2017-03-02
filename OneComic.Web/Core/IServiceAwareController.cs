using OneComic.Core;
using System.Collections.Generic;

namespace OneComic.Web.Core
{
    internal interface IServiceAwareController
    {
        void RegisterDisposableServices(List<IServiceContract> disposableServices);
        
        List<IServiceContract> DisposableServices { get; }
    }
}