using OneComic.Core;
using System.ComponentModel.DataAnnotations;

namespace OneComic.Data.Entities
{
    public class Book : IIdentifiableEntity
    {
        public int BookId { get; set; }

        public int ComicId { get; set; }

        public virtual Comic Comic { get; set; }

        [DataType(DataType.ImageUrl)]
        public string CoverImageUri { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Translator { get; set; }

        public string PageUris { get; set; }

        public int EntityId => BookId;
    }
}
