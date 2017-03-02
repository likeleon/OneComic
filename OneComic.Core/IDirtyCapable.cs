using System.Collections.Generic;

namespace OneComic.Core
{
    public interface IDirtyCapable
    {
        bool IsDirty { get; }

        IEnumerable<IDirtyCapable> GetDirtyObjects();
        void CleanDirtyObjects();
    }
}
