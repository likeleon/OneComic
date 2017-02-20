using Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Web.Http.Dependencies;

namespace Core.Common.API
{
    public sealed class MefAPIDependencyResolver : IDependencyResolver
    {
        private readonly CompositionContainer _container;

        public MefAPIDependencyResolver(CompositionContainer container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            return _container.GetExportedValueByType(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetExportedValuesByType(serviceType);
        }
    }
}
