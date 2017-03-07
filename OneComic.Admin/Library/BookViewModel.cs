using OneComic.Core;
using OneComic.Data.DTO;

namespace OneComic.Admin.Library
{
    public sealed class BookViewModel : ObjectBase
    {
        public Book Book { get; }

        public BookViewModel(Book book)
        {
            Book = book;
        }
    }
}
