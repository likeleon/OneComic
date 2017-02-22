using Core.Common.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System;

namespace OneComic.Business.Tests
{
    [TestClass]
    public class OneComicEngineTests
    {
        [TestMethod]
        public void AddBookmark()
        {
            int pageNumber = 5;

            var book = new Book
            {
                BookId = 100,
                ComicId = 1000,
                PageCount = pageNumber + 1
            };

            var mockRepositoryFactory = new Mock<IDataRepositoryFactory>();

            var mockBookRepository = new Mock<IBookRepository>();
            mockBookRepository.Setup(r => r.Get(book.BookId)).Returns(book);
            mockRepositoryFactory
                .Setup(obj => obj.GetDataRepository<IBookRepository>())
                .Returns(mockBookRepository.Object);

            var mockBookmarkRepository = new Mock<IBookmarkRepository>();
            mockBookmarkRepository
                .Setup(obj => obj.Add(It.IsAny<Bookmark>()))
                .Callback<Bookmark>(b => b.BookmarkId = 100)
                .Returns<Bookmark>(b => new RepositoryActionResult<Bookmark>(b, RepositoryActionState.Created));
            mockRepositoryFactory
                .Setup(obj => obj.GetDataRepository<IBookmarkRepository>())
                .Returns(mockBookmarkRepository.Object);

            var engine = new OneComicEngine(mockRepositoryFactory.Object);
            var account = new Account { AccountId = 10, LoginEmail = "a@a.a", };
            var bookmark = engine.AddBookmark(account, book.BookId, pageNumber);

            Assert.AreEqual(100, bookmark.BookmarkId);
            Assert.AreEqual(10, bookmark.AccountId);
            Assert.AreEqual(book.BookId, bookmark.BookId);
            Assert.AreEqual(pageNumber, bookmark.PageNumber);
            Assert.AreNotEqual(default(DateTime), bookmark.DateCreated);
        }
    }
}
