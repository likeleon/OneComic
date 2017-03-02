using System;
using System.Collections.Generic;

namespace OneComic.Data
{
    public interface IDataFields
    {
        Type Type { get; }
        IEnumerable<string> Fields { get; }
        IReadOnlyDictionary<string, IDataFields> AssociatedFields { get; }
    }
}
