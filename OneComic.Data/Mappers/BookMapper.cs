using Core.Common.Data;
using OneComic.Business.Entities;
using OneComic.Data.Contracts;
using System.ComponentModel.Composition;

namespace OneComic.Data.Mappers
{
    [Export(typeof(IBookMapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class BookMapper : DataMapper<Book, DTO.Book>, IBookMapper
    {
        public override DTO.Book ToDTO(Book book)
        {
            return new DTO.Book
            {
                BookId = book.BookId,
                ComicId = book.ComicId,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                Translator = book.Translator,
                PageCount = book.PageCount
            };
        }

        public override Book ToEntity(DTO.Book book)
        {
            return new Book
            {
                BookId = book.BookId,
                ComicId = book.ComicId,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                Translator = book.Translator,
                PageCount = book.PageCount
            };
        }
    }
}
