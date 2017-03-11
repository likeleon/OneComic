using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OneComic.Admin.Commands
{
    public sealed class NotifyTask : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Task Task { get; }
        public Task TaskCompleted { get; }

        public TaskStatus Status => Task.Status;
        public bool IsCompleted => Task.IsCompleted;
        public bool IsNotCompleted => !IsCompleted;
        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
        public bool IsCanceled => Task.IsCanceled;
        public bool IsFaulted => Task.IsFaulted;
        public AggregateException Exception => Task.Exception;
        public Exception InnerException => Exception?.InnerException;
        public string ErrorMessage => InnerException?.Message;

        public static NotifyTask Create(Task task)
        {
            return new NotifyTask(task);
        }

        public static NotifyTask<T> Create<T>(Task<T> task, T defaultResult = default(T))
        {
            return new NotifyTask<T>(task, defaultResult);
        }

        public static NotifyTask Create(Func<Task> asyncAction)
        {
            return new NotifyTask(asyncAction());
        }

        public static NotifyTask<T> Create<T>(Func<Task<T>> asyncAction, T defaultResult = default(T))
        {
            return new NotifyTask<T>(asyncAction(), defaultResult);
        }

        private NotifyTask(Task task)
        {
            Task = task;
            TaskCompleted = MonitorTaskAsync(task);
        }

        private async Task MonitorTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
            finally
            {
                NotifyPropertiesChanged(task);
            }
        }

        private void NotifyPropertiesChanged(Task task)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Exception)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(InnerException)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
            }
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsNotCompleted)));
        }
    }

    public sealed class NotifyTask<T> : INotifyPropertyChanged
    {
        private readonly T _defaultResult;

        public event PropertyChangedEventHandler PropertyChanged;

        public Task<T> Task { get; }
        public Task TaskCompleted { get; }

        public TaskStatus Status => Task.Status;
        public bool IsCompleted => Task.IsCompleted;
        public bool IsNotCompleted => !IsCompleted;
        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
        public bool IsCanceled => Task.IsCanceled;
        public bool IsFaulted => Task.IsFaulted;
        public AggregateException Exception => Task.Exception;
        public Exception InnerException => Exception?.InnerException;
        public string ErrorMessage => InnerException?.Message;

        internal NotifyTask(Task<T> task, T defaultResult)
        {
            _defaultResult = defaultResult;
            Task = task;
            TaskCompleted = MonitorTaskAsync(task);
        }

        private async Task MonitorTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
            finally
            {
                NotifyPropertiesChanged(task);
            }
        }

        private void NotifyPropertiesChanged(Task task)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Exception)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(InnerException)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsSuccessfullyCompleted)));
            }
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsNotCompleted)));
        }
    }
}
