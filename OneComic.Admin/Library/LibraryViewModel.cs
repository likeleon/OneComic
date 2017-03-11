﻿using Caliburn.Micro;
using Microsoft.VisualStudio.Language.Intellisense;
using OneComic.Admin.MainWindow;
using OneComic.Admin.Services;
using OneComic.API.Client;
using OneComic.Data.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OneComic.Admin.Library
{
    [Export(typeof(IMainScreenTabItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class LibraryViewModel : Screen, IMainScreenTabItem
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly OneComicClient _client = new OneComicClient("https://localhost:44304/api/");
        private object _selectedItem;

        public override string DisplayName
        {
            get { return "LIBARARY"; }
            set { }
        }

        public BulkObservableCollection<ComicViewModel> Comics { get; } = new BulkObservableCollection<ComicViewModel>();
        public BulkObservableCollection<BookViewModel> Books { get; } = new BulkObservableCollection<BookViewModel>();

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
            if (Execute.InDesignMode)
            {
                Comics.AddRange(LoadDesignModeData());
                SelectedItem = Comics.Last();
            }
        }

        [ImportingConstructor]
        public LibraryViewModel(IMessageBoxService messageBoxService)
        {
            _messageBoxService = messageBoxService;
        }

        protected override async void OnViewReady(object view)
        {
            base.OnViewReady(view);

            Comic[] comics = null;
            try
            {
                comics = await _client.GetComics(Enumerable.Empty<string>());
            }
            catch (Exception e)
            {
                _messageBoxService.Show(e.Message, "API Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var comicViewModels = comics.Select(comic => new ComicViewModel(comic));
            Comics.AddRange(comicViewModels);
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
