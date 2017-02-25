using Core.Common.Contracts;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Core.Common.Data
{
    public abstract class DataMapper<Entity, DTO> : IDataMapper<Entity, DTO>
        where Entity : class
        where DTO : class
    {
        private readonly IReadOnlyDictionary<string, PropertyInfo> _propertyInfoByFieldName = typeof(DTO).GetDtoFieldsWithPropertyInfo();

        public abstract DTO ToDTO(Entity entity);
        public abstract Entity ToEntity(DTO dto);

        public IEnumerable<string> Fields => _propertyInfoByFieldName.Keys;

        public object ToDataShapedObject(DTO dto, IEnumerable<string> fields)
        {
            if (fields?.Any() != true)
                return dto;

            var obj = new ExpandoObject() as IDictionary<string, object>;
            foreach (var field in fields)
            {
                var value = _propertyInfoByFieldName[field].GetValue(dto);
                obj.Add(field, value);
            }

            return obj;
        }

        public object ToDataShapedObject(Entity entity, IEnumerable<string> fields)
        {
            return ToDataShapedObject(ToDTO(entity), fields);
        }
    }
}
