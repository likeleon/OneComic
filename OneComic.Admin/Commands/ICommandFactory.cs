using System;
using System.Windows.Input;

namespace OneComic.Admin.Commands
{
    interface ICommandFactory
    {
        ICommand Create(Action execute);
        ICommand Create(Action execute, Func<bool> canExecute);
        ICommand Create<T>(Action<T> execute);
        ICommand Create<T>(Action<T> execute, Func<T, bool> canExecute);
    }
}
