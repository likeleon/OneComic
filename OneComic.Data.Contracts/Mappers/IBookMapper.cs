using OneComic.Business.Entities;

namespace OneComic.Data.Contracts
{
    public interface IBookMapper
    {
        DTO.Book ToDTO(Book book);
        Book ToEntity(DTO.Book book);
    }
}
