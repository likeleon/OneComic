using Microsoft.VisualStudio.Language.Intellisense;
using OneComic.Core;
using OneComic.Data.DTO;

namespace OneComic.Admin.Library
{
    public sealed class ComicViewModel : ObjectBase
    {
        public Comic Comic { get; }

        public BulkObservableCollection<BookViewModel> Books { get; } = new BulkObservableCollection<BookViewModel>();

        public ComicViewModel(Comic comic)
        {
            Comic = comic;
        }
    }
}
