using Core.Common.Contracts;
using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OneComic.Business.Common;
using OneComic.Business.Entities;
using OneComic.Business.Managers.Managers;
using OneComic.Common;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Security.Principal;
using System.Threading;

namespace OneComic.Business.Managers.Tests
{
    [TestClass]
    public class BookmarkManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(OneComicEngine).Assembly));
            Global.Container = new CompositionContainer(catalog);

            var principal = new GenericPrincipal(new GenericIdentity("likeleon"), new[] { "Administrators", Security.OneComicAdminRole });
            Thread.CurrentPrincipal = principal;
        }

        [TestMethod]
        public void AddBookmark()
        {
            var account = new Account
            {
                AccountId = 10,
                LoginEmail = "a@a.a"
            };

            int bookId = 100;
            int bookmarkId = 999;

            var mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();

            var mockAccountRepository = new Mock<IAccountRepository>();
            mockAccountRepository.Setup(r => r.GetByLoginEmail(account.LoginEmail)).Returns(account);
            mockDataRepositoryFactory.Setup(f => f.GetDataRepository<IAccountRepository>())
                .Returns(mockAccountRepository.Object);

            var mockBookmarkRepository = new Mock<IBookmarkRepository>();
            mockBookmarkRepository.Setup(r => r.Add(It.IsAny<Bookmark>()))
                .Callback<Bookmark>(b => b.BookmarkId = bookmarkId)
                .Returns<Bookmark>(b => b);

            mockDataRepositoryFactory.Setup(f => f.GetDataRepository<IBookmarkRepository>())
                .Returns(mockBookmarkRepository.Object);
            AttributedModelServices.ComposeExportedValue(Global.Container, mockDataRepositoryFactory.Object);

            int pageNumber = 99;
            
            var manager = new BookmarkManager();

            var addedBookmark = manager.AddBookmark(account.LoginEmail, bookId, pageNumber);
            Assert.AreEqual(bookmarkId, addedBookmark.BookmarkId);
            Assert.AreEqual(account.AccountId, addedBookmark.AccountId);
            Assert.AreEqual(bookId, addedBookmark.BookId);
            Assert.AreEqual(pageNumber, addedBookmark.PageNumber);
        }
    }
}
