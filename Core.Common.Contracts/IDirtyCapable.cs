using System.Collections.Generic;

namespace Core.Common.Contracts
{
    public interface IDirtyCapable
    {
        bool IsDirty { get; }

        IEnumerable<IDirtyCapable> GetDirtyObjects();
        void CleanDirtyObjects();
    }
}
