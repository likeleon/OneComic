using Core.Common.Core;
using FluentValidation;

namespace OneComic.Client.Entities
{
    public sealed class Book : ObjectBase
    {
        private int _bookId;
        private string _title;
        private string _description;
        private string _author;
        private string _translator;

        public int BookId
        {
            get { return _bookId; }
            set { Set(ref _bookId, value); }
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

        private class BookValidator : AbstractValidator<Book>
        {
            public BookValidator()
            {
                RuleFor(b => b.Title).NotEmpty();
            }
        };

        protected override IValidator CreateValidator()
        {
            return new BookValidator();
        }
    }
}
