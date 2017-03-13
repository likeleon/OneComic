using OneComic.Core;
using OneComic.Data.DTO;

namespace OneComic.Admin.Library
{
    public sealed class BookViewModel : ObjectBase
    {
        public Book Book { get; }
        public ComicViewModel ParentComicViewModel { get; }

        public BookViewModel(Book book, ComicViewModel parentComicViewModel)
        {
            Book = book;
            ParentComicViewModel = parentComicViewModel;
        }
    }
}
