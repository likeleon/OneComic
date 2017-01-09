using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OneComic.Client.Proxies.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void TestLibraryClientConnection()
        {
            var proxy = new LibraryClient();
            proxy.Open();
        }

        [TestMethod]
        public void TestBookmarkClientConnection()
        {
            var proxy = new BookmarkClient();
            proxy.Open();
        }

        [TestMethod]
        public void TestAccountClientConnection()
        {
            var proxy = new AccountClient();
            proxy.Open();
        }
    }
}
