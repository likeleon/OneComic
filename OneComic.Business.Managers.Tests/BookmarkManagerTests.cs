using Core.Common.Contracts;
using Core.Common.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
            int bookmarkId = 999;
            int pageNumber = 99;

            var account = new Account
            {
                AccountId = 10,
                LoginEmail = "a@a.a"
            };

            var book = new Book
            {
                BookId = 100,
                ComicId = 1000,
                PageCount = pageNumber + 1
            };

            var mockDataRepositoryFactory = new Mock<IDataRepositoryFactory>();
            AttributedModelServices.ComposeExportedValue(Global.Container, mockDataRepositoryFactory.Object);

            var mockAccountRepository = new Mock<IAccountRepository>();
            mockAccountRepository.Setup(r => r.GetByLoginEmail(account.LoginEmail)).Returns(account);
            mockDataRepositoryFactory.Setup(f => f.GetDataRepository<IAccountRepository>())
                .Returns(mockAccountRepository.Object);

            var mockBookRepository = new Mock<IBookRepository>();
            mockBookRepository.Setup(r => r.Get(book.BookId)).Returns(book);
            mockDataRepositoryFactory.Setup(f => f.GetDataRepository<IBookRepository>())
                .Returns(mockBookRepository.Object);

            var mockBookmarkRepository = new Mock<IBookmarkRepository>();
            mockBookmarkRepository.Setup(r => r.Add(It.IsAny<Bookmark>()))
                .Callback<Bookmark>(b => b.BookmarkId = bookmarkId)
                .Returns<Bookmark>(b => b);
            mockDataRepositoryFactory.Setup(f => f.GetDataRepository<IBookmarkRepository>())
                .Returns(mockBookmarkRepository.Object);

            
            var manager = new BookmarkManager();

            var addedBookmark = manager.AddBookmark(account.LoginEmail, book.BookId, pageNumber);
            Assert.AreEqual(bookmarkId, addedBookmark.BookmarkId);
            Assert.AreEqual(account.AccountId, addedBookmark.AccountId);
            Assert.AreEqual(book.BookId, addedBookmark.BookId);
            Assert.AreEqual(pageNumber, addedBookmark.PageNumber);
        }
    }
}
