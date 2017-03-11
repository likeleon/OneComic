using OneComic.Data.Entities;
using System;
using System.ComponentModel.Composition;

namespace OneComic.Data
{
    [Export(typeof(IBookMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class BookMapper : DataMapper<Book, DTO.Book>, IBookMapper
    {
        public override DTO.Book ToDTO(Book book)
        {
            var dto = new DTO.Book
            {
                BookId = book.BookId,
                ComicId = book.ComicId,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                Translator = book.Translator,
                PageCount = book.PageCount
            };
            if (book.CoverImageUri != null)
                dto.CoverImageUri = new Uri(book.CoverImageUri);
            return dto;
        }

        public override Book ToEntity(DTO.Book book)
        {
            return new Book
            {
                BookId = book.BookId,
                ComicId = book.ComicId,
                CoverImageUri = book.CoverImageUri?.AbsoluteUri,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                Translator = book.Translator,
                PageCount = book.PageCount
            };
        }
    }
}
