using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OneComic.Admin.Commands
{
    public interface ICommandFactory
    {
        ICommand Create(Action execute);
        ICommand Create(Action execute, Func<bool> canExecute);
        ICommand Create<T>(Action<T> execute);
        ICommand Create<T>(Action<T> execute, Func<T, bool> canExecute);

        IAsyncCommand CreateAsync(Func<Task> execute);
        IAsyncCommand CreateAsync(Func<Task> execute, Func<bool> canExecute);
        IAsyncCommand CreateAsync<T>(Func<T, Task> execute);
        IAsyncCommand CreateAsync<T>(Func<T, Task> execute, Func<T, bool> canExecute);
    }
}
