using System.Collections.Generic;

namespace OneComic.Data
{
    public interface IDataMapper<Entity, DTO> 
        where Entity: class
        where DTO : class
    {
        DTO ToDTO(Entity entity);
        Entity ToEntity(DTO dto);

        IEnumerable<string> Fields { get; }

        object ToDataShapedObject(Entity entity, IDataFields fields);
    }
}
