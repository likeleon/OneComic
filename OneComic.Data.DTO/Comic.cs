using System;

namespace OneComic.Data.DTO
{
    public sealed class Comic
    {
        public int ComicId { get; set; }
        public Uri CoverImageUri { get; set; }
        public string Title { get; set; }
    }
}
