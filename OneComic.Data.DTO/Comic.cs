using FluentValidation;
using OneComic.Core;
using System;

namespace OneComic.Data.DTO
{
    public sealed class Comic : ObjectBase
    {
        private int _comicId;
        private Uri _coverImageUri;
        private string _title;

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

        private sealed class ComicValidator : AbstractValidator<Comic>
        {
            public ComicValidator()
            {
                RuleFor(comic => comic.Title).NotEmpty();
            }
        }

        protected override IValidator CreateValidator()
        {
            return new ComicValidator();
        }
    }
}
