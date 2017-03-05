using OneComic.Core;
using OneComic.Data.DTO;
using System.Collections.ObjectModel;

namespace OneComic.Admin.Library
{
    public sealed class ComicViewModel : ObjectBase
    {
        public Comic Comic { get; }
        public ObservableCollection<BookViewModel> Books { get; } = new ObservableCollection<BookViewModel>();

        public bool IsExpanded { get; set; }

        public ComicViewModel(Comic comic)
        {
            Comic = comic;
        }
    }
}
