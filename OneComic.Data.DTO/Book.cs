using FluentValidation;
using OneComic.Core;
using System;

namespace OneComic.Data.DTO
{
    public sealed class Book : ObjectBase
    {
        private int _bookId;
        private int _comicId;
        private Uri _coverImageUri;
        private string _title;
        private string _description;
        private string _author;
        private string _translator;
        private int _pageCount;

        public int BookId
        {
            get { return _bookId; }
            set { Set(ref _bookId, value); }
        }

        public int ComicId
        {
            get { return _comicId; }
            set { Set(ref _comicId, value); }
        }

        public Uri CoverImageUri
        {
            get { return _coverImageUri; }
            set { Set(ref _coverImageUri, value); }
        }

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        public string Author
        {
            get { return _author; }
            set { Set(ref _author, value); }
        }

        public string Translator
        {
            get { return _translator; }
            set { Set(ref _translator, value); }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set { Set(ref _pageCount, value); }
        }

        private sealed class BookValidator : AbstractValidator<Book>
        {
            public BookValidator()
            {
                RuleFor(book => book.Title).NotEmpty();
            }
        }
    }
}
