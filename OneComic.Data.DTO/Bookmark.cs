using System;

namespace OneComic.Data.DTO
{
    public sealed class Bookmark
    {
        public int BookmarkId { get; set; }
        public int AccountId { get; set; }
        public int BookId { get; set; }
        public int PageNumber { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
