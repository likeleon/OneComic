using OneComic.Business.Entities;

namespace OneComic.Data.Contracts
{
    public interface IBookmarkMapper
    {
        DTO.Bookmark ToDTO(Bookmark bookmark);
        Bookmark ToEntity(DTO.Bookmark bookmark);
    }
}
