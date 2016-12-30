using OneComic.Data;
using System.ComponentModel.Composition.Hosting;

namespace OneComic.Business.Bootstrapper
{
    public static class MefLoader
    {
        public static CompositionContainer Init()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));

            return new CompositionContainer(catalog);
        }
    }
}
