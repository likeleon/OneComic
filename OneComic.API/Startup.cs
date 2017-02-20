using Core.Common.API;
using Microsoft.Owin;
using OneComic.Client.Bootstrapper;
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
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            var container = MefLoader.Init(catalog.Catalogs);

            GlobalConfiguration.Configuration.DependencyResolver = new MefAPIDependencyResolver(container);
        }
    }
}