using System.Collections.Generic;

namespace OneComic.Data
{
    public class DataFields<T> : DataFields
    {
        public DataFields()
            : base(typeof(T))
        {
        }

        public DataFields(IEnumerable<string> fields)
            : base(typeof(T), fields)
        {
        }

        public DataFields(IEnumerable<string> fields, IReadOnlyDictionary<string, IDataFields> associatedFields)
            : base(typeof(T), fields, associatedFields)
        {
        }
    }
}
