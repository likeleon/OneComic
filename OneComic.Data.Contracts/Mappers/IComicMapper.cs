using OneComic.Business.Entities;
using System.Collections.Generic;

namespace OneComic.Data.Contracts
{
    public interface IComicMapper
    {
        DTO.Comic ToDTO(Comic comic);
        Comic ToEntity(DTO.Comic comic);

        bool HasProperty(string property);
        object ToDataShapedObject(Comic comic, IEnumerable<string> properties);
        object ToDataShapedObject(DTO.Comic comic, IEnumerable<string> properties);
    }
}
