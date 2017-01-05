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
        public void TestMethod1()
        {
            var mockBookmarkRepository = new Mock<IBookmarkRepository>();
            mockBookmarkRepository
                .Setup(obj => obj.Add(It.IsAny<Bookmark>()))
                .Callback<Bookmark>(b => b.BookmarkId = 100)
                .Returns<Bookmark>(b => b);

            var mockRepositoryFactory = new Mock<IDataRepositoryFactory>();
            mockRepositoryFactory
                .Setup(obj => obj.GetDataRepository<IBookmarkRepository>())
                .Returns(mockBookmarkRepository.Object);

            var engine = new OneComicEngine(mockRepositoryFactory.Object);
            var account = new Account { AccountId = 10, LoginEmail = "a@a.a", };
            var bookmark = engine.AddBookmark(account, bookId: 3, pageNumber: 5);

            Assert.AreEqual(100, bookmark.BookmarkId);
            Assert.AreEqual(10, bookmark.AccountId);
            Assert.AreEqual(3, bookmark.BookId);
            Assert.AreEqual(5, bookmark.PageNumber);
            Assert.AreNotEqual(default(DateTime), bookmark.DateCreated);
        }
    }
}
