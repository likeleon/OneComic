using OneComic.Core;

namespace OneComic.Business.Entities
{
    public class Book : IIdentifiableEntity
    {
        public int BookId { get; set; }

        public int ComicId { get; set; }

        public virtual Comic Comic { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Translator { get; set; }

        public int PageCount { get; set; }

        public int EntityId => BookId;
    }
}
