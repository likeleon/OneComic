using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Common.Data
{
    public class DataFields : IDataFields
    {
        public IReadOnlyDictionary<string, IDataFields> AssociatedFields { get; }
        public IEnumerable<string> Fields { get; }
        public Type Type { get; }

        public DataFields(Type type)
            : this(type, type.GetDtoFields(), new Dictionary<string, IDataFields>())
        {
        }

        public DataFields(Type type, IEnumerable<string> fields)
            : this(type, fields, new Dictionary<string, IDataFields>())
        {
        }

        public DataFields(Type type, IEnumerable<string> fields, IReadOnlyDictionary<string, IDataFields> associatedFields)
        {
            Type = type;
            Fields = fields.ToArray();
            AssociatedFields = associatedFields;
        }
    }
}
