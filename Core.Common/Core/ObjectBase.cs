using Core.Common.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Core.Common.Core
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable
    {
        private bool _isDirty;

        public bool IsDirty
        {
            get { return _isDirty; }
            set { Set(ref _isDirty, value, makeDirty: false); }
        }

        public override bool Set<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            return Set(ref currentValue, newValue, makeDirty: true, propertyName: propertyName);
        }

        public bool Set<T>(ref T currentValue, T newValue, bool makeDirty, [CallerMemberName] string propertyName = "")
        {
            if (!base.Set(ref currentValue, newValue, propertyName))
                return false;

            if (makeDirty)
                IsDirty = true;

            return true;
        }

        public IEnumerable<IDirtyCapable> GetDirtyObjects()
        {
            return ObjectGraph.GetSelfAndDescendants(this).Where(o => o.IsDirty);
        }

        public void CleanDirtyObjects()
        {
            GetDirtyObjects().OfType<ObjectBase>().Do(o => o.IsDirty = false);
        }
    }
}
