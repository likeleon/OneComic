using Caliburn.Micro;
using OneComic.Admin.MainWindow;
using OneComic.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OneComic.Admin.Library
{
    [Export(typeof(IMainScreenTabItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class LibraryViewModel : Screen, IMainScreenTabItem
    {
        private object _selectedItem;

        public override string DisplayName
        {
            get { return "LIBARARY"; }
            set { }
        }

        public IEnumerable<ComicViewModel> Comics { get; }
        public IEnumerable<BookViewModel> Books { get; }

        public ComicViewModel SelectedComic => SelectedItem as ComicViewModel;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Set(ref _selectedItem, value))
                    NotifyOfPropertyChange(nameof(SelectedComic));
            }
        }

        public LibraryViewModel()
        {
            //if (Execute.InDesignMode)
            {
                Comics = LoadDesignModeData().ToArray();
                SelectedItem = Comics.Last();
            }
        }

        private IEnumerable<ComicViewModel> LoadDesignModeData()
        {
            for (var i = 0; i < 5; ++i)
            {
                var comic = new Comic
                {
                    ComicId = i,
                    CoverImageUri = new Uri("http://cfile2.uf.tistory.com/image/1973EC3F4EC116C22D67F9"),
                    Title = $"은혼 {i}"
                };
                var comicViewModel = new ComicViewModel(comic);

                for (var j = 0; j < 10; ++j)
                {
                    var book = new Book
                    {
                        BookId = j,
                        ComicId = comic.ComicId,
                        CoverImageUri = new Uri("http://misc.ridibooks.com/cover/1019000311/xxlarge"),
                        Title = $"은혼 {i} - {j + 1}권"
                    };
                    var bookViewModel = new BookViewModel(book);
                    comicViewModel.Books.Add(bookViewModel);
                }
                yield return comicViewModel;
            }
        }

        private bool Set<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;

            NotifyOfPropertyChange(propertyName);
            return true;
        }
    }
}
