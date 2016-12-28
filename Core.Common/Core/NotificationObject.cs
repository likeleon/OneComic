using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Core.Common.Core
{
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler _innerEvent;

        private readonly HashSet<PropertyChangedEventHandler> _subscribers = new HashSet<PropertyChangedEventHandler>();

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (_subscribers.Add(value))
                    _innerEvent += value;
            }
            remove
            {
                if (_subscribers.Remove(value))
                    _innerEvent -= value;
            }
        }

        public virtual bool Set<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;

            currentValue = newValue;

            RaisePropertyChanged(propertyName);
            return true;
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            _innerEvent?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
