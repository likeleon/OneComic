using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;

namespace OneComic.Data.Mappers
{
    [Export(typeof(IBookmarkMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class BookmarkMapper : IBookmarkMapper
    {
        public DTO.Bookmark ToDTO(Bookmark bookmark)
        {
            return new DTO.Bookmark
            {
                BookmarkId = bookmark.BookmarkId,
                AccountId = bookmark.AccountId,
                BookId = bookmark.BookId,
                PageNumber = bookmark.PageNumber,
                DateCreated = bookmark.DateCreated
            };
        }

        public Bookmark ToEntity(DTO.Bookmark bookmark)
        {
            return new Bookmark
            {
                BookmarkId = bookmark.BookmarkId,
                AccountId = bookmark.AccountId,
                BookId = bookmark.BookId,
                PageNumber = bookmark.PageNumber,
                DateCreated = bookmark.DateCreated
            };
        }
    }
}
