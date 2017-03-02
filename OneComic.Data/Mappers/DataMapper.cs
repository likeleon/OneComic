using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace OneComic.Data
{
    public abstract class DataMapper<Entity, DTO> : IDataMapper<Entity, DTO>
        where Entity : class
        where DTO : class
    {
        private readonly IReadOnlyDictionary<string, PropertyInfo> _propertyInfoByFieldName = typeof(DTO).GetDtoFieldsWithPropertyInfo();

        public abstract DTO ToDTO(Entity entity);
        public abstract Entity ToEntity(DTO dto);

        public IEnumerable<string> Fields => _propertyInfoByFieldName.Keys;

        public object ToDataShapedObject(Entity entity, IDataFields fields)
        {
            var dto = ToDTO(entity);
            if (fields == null)
                return dto;

            var obj = new ExpandoObject() as IDictionary<string, object>;
            foreach (var field in fields.Fields)
            {
                var value = _propertyInfoByFieldName[field].GetValue(dto);
                obj.Add(field, value);
            }

            foreach (var kvp in fields.AssociatedFields)
            {
                var associationName = kvp.Key;
                var associatedFields = kvp.Value;
                var associatedObject = CreateAssociatedObject(entity, associationName, associatedFields);
                obj.Add(associationName, associatedObject);
            }

            return obj;
        }

        protected virtual object CreateAssociatedObject(Entity entity, string associationName, IDataFields fields)
        {
            throw new NotImplementedException($"Association not supported: {associationName}");
        }
    }
}
