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

        public override string DisplayName
        {
            get { return "LIBARARY"; }
            set { }
        }

        public BulkObservableCollection<ComicViewModel> Comics { get; } = new BulkObservableCollection<ComicViewModel>();

        public ComicViewModel SelectedComic => SelectedItem as ComicViewModel;
        public BookViewModel SelectedBook => SelectedItem as BookViewModel;

        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Set(ref _selectedItem, value))
                {
                    NotifyOfPropertyChange(nameof(SelectedComic));
                    NotifyOfPropertyChange(nameof(SelectedBook));
                }
            }
        }

        public IAsyncCommand GetComicsCommand { get; }
        public IAsyncCommand AddComicCommand { get; }
        public IAsyncCommand DeleteSelectedItemCommand { get; }
        public IAsyncCommand AddBookCommand { get; }
        public IAsyncCommand SaveSelectedItemCommand { get; }

        public LibraryViewModel()
        {
            if (Execute.InDesignMode)
            {
                Comics.AddRange(LoadDesignModeData());
                SelectedItem = Comics.Last();
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
            DeleteSelectedItemCommand = commandFactory.CreateAsync(DeleteSelectedItem, () => SelectedItem != null);
            AddBookCommand = commandFactory.CreateAsync(AddBook, () => SelectedComic != null);
            SaveSelectedItemCommand = commandFactory.CreateAsync(SaveSelectedItem, CanSaveSelectedItem);
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

            var comicViewModels = new List<ComicViewModel>();

            var comics = await _client.GetComics(Enumerable.Empty<string>());
            foreach (var comic in comics)
            {
                var comicViewModel = new ComicViewModel(comic);

                var books = await _client.GetBooks(comic.ComicId);
                var bookViewModels = books.Select(book => new BookViewModel(book, comicViewModel));
                comicViewModel.Books.AddRange(bookViewModels);

                comicViewModels.Add(comicViewModel);
            }

            Comics.AddRange(comicViewModels);
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

        private async Task DeleteSelectedItem()
        {
            if (SelectedComic != null)
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
            else if (SelectedBook != null)
            {
                var dialogResult = await _dialogCoordinator.ShowMessageAsync(
                    context: this,
                    title: "Delete a book",
                    message: $"Delete book '{SelectedBook.Book.Title}'?",
                    style: MessageDialogStyle.AffirmativeAndNegative);
                if (dialogResult != MessageDialogResult.Affirmative)
                    return;

                await _client.DeleteBook(SelectedBook.Book.BookId);
                SelectedBook.ParentComicViewModel.Books.Remove(SelectedBook);
            }
        }

        private async Task AddBook()
        {
            var bookTitle = await _dialogCoordinator.ShowInputAsync(this, "Add a new book", "Book Title");
            if (bookTitle.IsNullOrEmpty())
                return;

            var book = new Book
            {
                ComicId = SelectedComic.Comic.ComicId,
                Title = bookTitle
            };
            book = await _client.AddBook(book);
            SelectedComic.Books.Add(new BookViewModel(book, SelectedComic));
        }

        private async Task SaveSelectedItem()
        {
            if (SelectedComic != null)
                await _client.SaveComic(SelectedComic.Comic);
        }

        private bool CanSaveSelectedItem()
        {
            return SelectedComic?.Comic?.IsDirty == true ||
                SelectedBook?.Book?.IsDirty == true;
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
                    var bookViewModel = new BookViewModel(book, comicViewModel);
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
