using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OneComic.Business.Bootstrapper;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;

namespace OneComic.Data.Tests
{
    [TestClass]
    public class DataLayerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Global.Container = MefLoader.Init();
        }

        [TestMethod]
        public void RepositoryFactory()
        {
            var mockAccountRepository = new Mock<IAccountRepository>();
            AttributedModelServices.ComposeExportedValue(Global.Container, mockAccountRepository.Object);

            var accounts = new[]
            {
                new Account { AccountId = 1, LoginEmail = "a@a.a" },
                new Account { AccountId = 2, LoginEmail = "b@b.b" }
            };
            mockAccountRepository.Setup(r => r.Get()).Returns(accounts);

            var repositoryFactory = new DataRepositoryFactory();
            Assert.AreEqual(accounts, repositoryFactory.GetDataRepository<IAccountRepository>().Get());
        }
    }
}
