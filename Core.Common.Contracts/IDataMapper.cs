using System.Collections.Generic;

namespace Core.Common.Contracts
{
    public interface IDataMapper<Entity, DTO> 
        where Entity: class
        where DTO : class
    {
        DTO ToDTO(Entity entity);
        Entity ToEntity(DTO dto);

        IEnumerable<string> Fields { get; }

        object ToDataShapedObject(Entity entity, IEnumerable<string> fields);
        object ToDataShapedObject(DTO dto, IEnumerable<string> fields);
    }
}
