using Core.Common.Contracts;
using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneComic.Client.Bootstrapper;
using OneComic.Client.Contracts;

namespace OneComic.Client.Proxies.Tests
{
    [TestClass]
    public class ProxyObtainmentTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Global.Container = MefLoader.Init();
        }

        [TestMethod]
        public void ObtainProxyFromContainerUsingServiceContract()
        {
            var proxy = Global.Container.GetExportedValue<ILibraryService>();
            Assert.IsInstanceOfType(proxy, typeof(LibraryClient));
        }

        [TestMethod]
        public void ObtainProxyFromServiceFactory()
        {
            var factory = new ServiceFactory();
            var proxy = factory.CreateClient<ILibraryService>();
            Assert.IsInstanceOfType(proxy, typeof(LibraryClient));
        }

        [TestMethod]
        public void ObtainServiceFactoryAndProxyFromContainer()
        {
            var factory = Global.Container.GetExportedValue<IServiceFactory>();
            var proxy = factory.CreateClient<ILibraryService>();
            Assert.IsInstanceOfType(proxy, typeof(LibraryClient));
        }
    }
}
