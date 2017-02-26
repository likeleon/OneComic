using System;
using System.Collections.Generic;

namespace Core.Common.Contracts
{
    public interface IDataFields
    {
        Type Type { get; }
        IEnumerable<string> Fields { get; }
        IReadOnlyDictionary<string, IDataFields> AssociatedFields { get; }
    }
}
