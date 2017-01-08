using OneComic.Client.Contracts;
using OneComic.Client.Entities;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading.Tasks;

namespace OneComic.Client.Proxies
{
    [Export(typeof(IBookmarkService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BookmarkClient : ClientBase<IBookmarkService>, IBookmarkService
    {
        public Bookmark AddBookmark(string loginEmail, int bookId, int pageNumber) 
            => Channel.AddBookmark(loginEmail, bookId, pageNumber);

        public Task<Bookmark> AddBookmarkAsync(string loginEmail, int bookId, int pageNumber)
            => Channel.AddBookmarkAsync(loginEmail, bookId, pageNumber);

        public AccountBookmarkData[] GetBookmarks(string loginEmail) => Channel.GetBookmarks(loginEmail);

        public Task<AccountBookmarkData[]> GetBookmarksAsync(string loginEmail) => Channel.GetBookmarksAsync(loginEmail);

        public void RemoveBookmark(int bookmarkId) => Channel.RemoveBookmark(bookmarkId);

        public Task RemoveBookmarkAsync(int bookmarkId) => Channel.RemoveBookmarkAsync(bookmarkId);
    }
}
