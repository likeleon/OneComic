using System.Collections.Generic;
using System.Linq;

namespace Core.Common.Contracts
{
    public static class IDataMapperExtensions
    {
        public static IEnumerable<object> ToDataShapedObjects<Entity, DTO>(
            this IDataMapper<Entity, DTO> mapper,
            IEnumerable<Entity> entities,
            IDataFields fields)
            where Entity : class
            where DTO : class
        {
            return entities.Select(entity => mapper.ToDataShapedObject(entity, fields));
        }
    }
}
