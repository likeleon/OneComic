using Core.Common.Core;
using FluentValidation;

namespace OneComic.Client.Entities
{
    public sealed class Comic : ObjectBase
    {
        private int _comicId;
        private string _title;

        public int ComicId
        {
            get { return _comicId; }
            set { Set(ref _comicId, value); }
        }

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        private class ComicValidator : AbstractValidator<Comic>
        {
            public ComicValidator()
            {
                RuleFor(c => c.Title).NotEmpty();
            }
        }

        protected override IValidator CreateValidator()
        {
            return new ComicValidator();
        }
    }
}
