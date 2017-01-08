using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneComic.Business.Contracts;
using System.ServiceModel;

namespace OneComic.ServiceHost.Tests
{
    [TestClass]
    public class ServiceAccessTests
    {
        [TestMethod]
        public void TestLibraryManagerAsService()
        {
            var channelFactory = new ChannelFactory<ILibraryService>("");
            var proxy = channelFactory.CreateChannel();
            (proxy as ICommunicationObject).Open();
            channelFactory.Close();
        }

        [TestMethod]
        public void TestBookmarkManagerAsService()
        {
            var channelFactory = new ChannelFactory<IBookmarkService>("");
            var proxy = channelFactory.CreateChannel();
            (proxy as ICommunicationObject).Open();
            channelFactory.Close();
        }

        [TestMethod]
        public void TestAccountManagerAsService()
        {
            var channelFactory = new ChannelFactory<IAccountService>("");
            var proxy = channelFactory.CreateChannel();
            (proxy as ICommunicationObject).Open();
            channelFactory.Close();
        }
    }
}
