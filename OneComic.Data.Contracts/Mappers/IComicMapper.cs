using OneComic.Business.Entities;
using System.Collections.Generic;

namespace OneComic.Data.Contracts
{
    public interface IComicMapper
    {
        DTO.Comic ToDTO(Comic comic);
        Comic ToEntity(DTO.Comic comic);

        object ToDataShapedObject(Comic comic, IEnumerable<string> fields);
        object ToDataShapedObject(DTO.Comic comic, IEnumerable<string> fields);
    }
}
