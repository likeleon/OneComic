using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.VisualStudio.Language.Intellisense;
using OneComic.Admin.Commands;
using OneComic.Admin.MainWindow;
using OneComic.Admin.Services;
using OneComic.API.Client;
using OneComic.Core;
using OneComic.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OneComic.Admin.Library
{
    [Export(typeof(IMainScreenTabItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class LibraryViewModel : Screen, IMainScreenTabItem
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IMessageBoxService _messageBoxService;
        private readonly OneComicClient _client = new OneComicClient("https://localhost:44304/api/");
        private ComicViewModel _selectedComic;

        public override string DisplayName
        {
            get { return "LIBARARY"; }
            set { }
        }

        public BulkObservableCollection<ComicViewModel> Comics { get; } = new BulkObservableCollection<ComicViewModel>();
        public BulkObservableCollection<BookViewModel> Books { get; } = new BulkObservableCollection<BookViewModel>();

        public ComicViewModel SelectedComic
        {
            get { return _selectedComic; }
            set
            {
                if (Set(ref _selectedComic, value))
                    NotifyOfPropertyChange(nameof(SelectedComic));
            }
        }

        public IAsyncCommand GetComicsCommand { get; }
        public IAsyncCommand AddComicCommand { get; }
        public IAsyncCommand DeleteComicCommand { get; }
        public IAsyncCommand AddBookCommand { get; }

        public LibraryViewModel()
        {
            if (Execute.InDesignMode)
            {
                Comics.AddRange(LoadDesignModeData());
                SelectedComic = Comics.Last();
            }
        }

        [ImportingConstructor]
        public LibraryViewModel(
            IDialogCoordinator dialogCoordinator,
            IMessageBoxService messageBoxService, 
            ICommandFactory commandFactory)
        {
            _dialogCoordinator = dialogCoordinator;
            _messageBoxService = messageBoxService;

            GetComicsCommand = commandFactory.CreateAsync(GetComics);
            AddComicCommand = commandFactory.CreateAsync(AddComic);
            DeleteComicCommand = commandFactory.CreateAsync(DeleteComic, () => SelectedComic != null);
            AddBookCommand = commandFactory.CreateAsync(AddBook, () => SelectedComic != null);
        }

        protected override async void OnViewReady(object view)
        {
            base.OnViewReady(view);

            if (GetComicsCommand.CanExecute(null))
                await GetComicsCommand.ExecuteAsync(null);
        }

        private async Task GetComics()
        {
            Comics.Clear();

            try
            {
                Comics.BeginBulkOperation();

                var comics = await _client.GetComics(Enumerable.Empty<string>());
                foreach (var comic in comics)
                {
                    var comicViewModel = new ComicViewModel(comic);

                    var books = await _client.GetBooks(comic.ComicId);
                    comicViewModel.Books.AddRange(books.Select(book => new BookViewModel(book)));

                    Comics.Add(comicViewModel);
                }
            }
            finally
            {
                Comics.EndBulkOperation();
            }
        }

        private async Task AddComic()
        {
            var comicTitle = await _dialogCoordinator.ShowInputAsync(this, "Add a new comic", "Comic Title");
            if (comicTitle.IsNullOrEmpty())
                return;

            var comic = new Comic { Title = comicTitle };
            comic = await _client.AddComic(comic);
            Comics.Add(new ComicViewModel(comic));
        }

        private async Task DeleteComic()
        {
            var dialogResult = await _dialogCoordinator.ShowMessageAsync(
                context: this, 
                title: "Delete a comic", 
                message: $"Delete comic '{SelectedComic.Comic.Title}'?", 
                style: MessageDialogStyle.AffirmativeAndNegative);
            if (dialogResult != MessageDialogResult.Affirmative)
                return;

            await _client.DeleteComic(SelectedComic.Comic.ComicId);
            Comics.Remove(SelectedComic);
        }

        private async Task AddBook()
        {
            var bookTitle = await _dialogCoordinator.ShowInputAsync(this, "Add a new book", "Book Title");
            if (bookTitle.IsNullOrEmpty())
                return;

            var book = new Book { Title = bookTitle };
            book = await _client.AddBook(book);
            SelectedComic.Books.Add(new BookViewModel(book));
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
