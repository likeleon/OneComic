using Core.Common.Contracts;
using System.Collections.Generic;

namespace OneComic.Business.Entities
{
    public class Comic : IIdentifiableEntity
    {
        public int ComicId { get; set; }

        public string Title { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public int EntityId => ComicId;
    }
}
