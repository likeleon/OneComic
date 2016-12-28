using Core.Common.Contracts;
using System.Runtime.CompilerServices;

namespace Core.Common.Core
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable
    {
        private bool _isDirty;

        public bool IsDirty
        {
            get { return _isDirty; }
            set { Set(ref _isDirty, value); }
        }

        public bool Set<T>(ref T currentValue, T newValue, bool setDirty, [CallerMemberName] string propertyName = "")
        {
            if (!Set(ref currentValue, newValue, propertyName))
                return false;

            if (setDirty)
                IsDirty = true;

            return true;
        }
    }
}
