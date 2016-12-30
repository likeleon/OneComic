using Core.Common.Contracts;
using Core.Common.Core;
using System.ComponentModel.Composition;

namespace OneComic.Business
{
    [Export(typeof(IBusinessEngineFactory))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class BusinessEngineFactory : IBusinessEngineFactory
    {
        T IBusinessEngineFactory.GetBusinessEngine<T>()
        {
            return Global.Container.GetExportedValue<T>();
        }
    }
}
