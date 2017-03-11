using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneComic.Admin.Commands
{
    [Export(typeof(ICommandFactory))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class CommandFactory : ICommandFactory
    {
        public ICommand Create(Action execute)
        {
            return new DelegateCommand(execute, null);
        }

        public ICommand Create(Action execute, Func<bool> canExecute)
        {
            return new DelegateCommand(execute, canExecute);
        }

        public ICommand Create<T>(Action<T> execute)
        {
            return new DelegateCommand<T>(execute, null);
        }

        public ICommand Create<T>(Action<T> execute, Func<T, bool> canExecute)
        {
            return new DelegateCommand<T>(execute, canExecute);
        }

        public IAsyncCommand CreateAsync(Func<Task> execute)
        {
            return new AsyncCommand(execute, null);
        }

        public IAsyncCommand CreateAsync(Func<Task> execute, Func<bool> canExecute)
        {
            return new AsyncCommand(execute, canExecute);
        }

        public IAsyncCommand CreateAsync<T>(Func<T, Task> execute)
        {
            return new AsyncCommand<T>(execute, null);
        }

        public IAsyncCommand CreateAsync<T>(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            return new AsyncCommand<T>(execute, canExecute);
        }
    }
}
