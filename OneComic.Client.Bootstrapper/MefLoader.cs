using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace OneComic.Client.Bootstrapper
{
    public static class MefLoader
    {
        public static CompositionContainer Init()
        {
            return Init(Enumerable.Empty<ComposablePartCatalog>());
        }

        public static CompositionContainer Init(IEnumerable<ComposablePartCatalog> catalogParts)
        {
            var catalog = new AggregateCatalog();

            foreach (var part in catalogParts)
                catalog.Catalogs.Add(part);

            return new CompositionContainer(catalog);
        }
    }
}
