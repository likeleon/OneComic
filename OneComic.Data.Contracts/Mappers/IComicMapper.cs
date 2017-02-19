using OneComic.Business.Entities;

namespace OneComic.Data.Contracts
{
    public interface IComicMapper
    {
        DTO.Comic ToDTO(Comic comic);
        Comic ToEntity(DTO.Comic comic);
    }
}
