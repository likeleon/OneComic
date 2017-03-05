using Caliburn.Micro;
using OneComic.Admin.MainWindow;
using OneComic.Data.DTO;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;

namespace OneComic.Admin.Library
{
    [Export(typeof(IMainScreenTabItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class LibraryViewModel : Screen, IMainScreenTabItem
    {
        public override string DisplayName
        {
            get { return "라이브러리"; }
            set { }
        }

        public IEnumerable<ComicViewModel> Comics { get; }

        public LibraryViewModel()
        {
            //if (Execute.InDesignMode)
                Comics = LoadDesignModeData();
        }

        private IEnumerable<ComicViewModel> LoadDesignModeData()
        {
            for (var i = 0; i < 5; ++i)
            {
                var comic = new Comic
                {
                    ComicId = i,
                    Title = $"만화 타이틀 {i}"
                };
                var comicViewModel = new ComicViewModel(comic)
                {
                    IsExpanded = true
                };

                for (var j = 0; j < 10; ++j)
                {
                    var book = new Book
                    {
                        Title = $"책 타이틀 {j}"
                    };
                    var bookViewModel = new BookViewModel(book)
                    {
                        IsExpanded = true
                    };
                    comicViewModel.Books.Add(bookViewModel);
                }
                yield return comicViewModel;
            }
        }
    }
}
