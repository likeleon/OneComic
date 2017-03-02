using OneComic.Business.Entities;
using System.ComponentModel.Composition;

namespace OneComic.Data
{
    [Export(typeof(IBookmarkMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class BookmarkMapper : DataMapper<Bookmark, DTO.Bookmark>, IBookmarkMapper
    {
        public override DTO.Bookmark ToDTO(Bookmark bookmark)
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

        public override Bookmark ToEntity(DTO.Bookmark bookmark)
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
