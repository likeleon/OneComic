using OneComic.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneComic.Data.Entities
{
    public class Comic : IIdentifiableEntity
    {
        public int ComicId { get; set; }

        [DataType(DataType.ImageUrl)]
        public string CoverImageUri { get; set; }

        public string Title { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public int EntityId => ComicId;
    }
}
