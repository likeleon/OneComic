using Core.Common.API;
using Core.Common.Data;
using Microsoft.Owin;
using OneComic.API.ModelBinders;
using OneComic.Client.Bootstrapper;
using OneComic.Data;
using Owin;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Web.Http;

[assembly: OwinStartup(typeof(OneComic.API.Startup))]
namespace OneComic.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();

            InstallDependencyResolver(configuration);

            WebApiConfig.Register(configuration);

            configuration.BindParameter(typeof(DataFields<Data.DTO.Comic>), new ComicFieldsModelBinder());
            configuration.BindParameter(typeof(DataFields<Data.DTO.Book>), new BookFieldsModelBinder());
            
            app.UseWebApi(configuration);
        }

        private void InstallDependencyResolver(HttpConfiguration configuration)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));

            var container = MefLoader.Init(catalog.Catalogs);
            var dependencyResolver = new MefAPIDependencyResolver(container);

            configuration.DependencyResolver = dependencyResolver;
        }
    }
}