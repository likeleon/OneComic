using Core.Common.Contracts;
using OneComic.Business.Entities;

namespace OneComic.Business.Common
{
    public interface IOneComicEngine : IBusinessEngine
    {
        bool IsPageNumberInRange(int bookId, int pageNumber);
        Bookmark AddBookmark(string loginEmail, int bookId, int pageNumber);
    }
}
